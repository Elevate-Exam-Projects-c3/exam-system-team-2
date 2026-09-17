using MediatR;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public class VerifyResetCodeOrchestratorHandler : IRequestHandler<VerifyResetCodeOrchestratorRequest, RequestResponse<string>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyResetCodeOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<string>> Handle(VerifyResetCodeOrchestratorRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        // 1. Precondition Sub-Query: Find Latest OTP
        var latestOtp = await _mediator.Send(new GetLatestPasswordResetOtpQuery(normalizedEmail), cancellationToken);
        if (latestOtp == null || latestOtp.IsUsed)
        {
            return RequestResponse<string>.Fail("Invalid or expired verification code.", 400);
        }

        if (DateTime.UtcNow > latestOtp.ExpiresAt)
        {
            return RequestResponse<string>.Fail("Verification code has expired. Please request a new one.", 400);
        }

        if (latestOtp.AttemptCount >= 5)
        {
            return RequestResponse<string>.Fail("Too many failed attempts. This code has been invalidated. Please request a new one.", 400);
        }

        // 2. Cryptographic Validation
        var isMatch = await _mediator.Send(new ValidateResetOtpHashQuery(request.Code, latestOtp.OtpHash), cancellationToken);

        if (!isMatch)
        {
            // Transaction to commit incremented attempt count atomically
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _mediator.Send(new RecordFailedResetCodeAttemptSubCommand(latestOtp), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                throw;
            }

            if (latestOtp.AttemptCount >= 5)
            {
                return RequestResponse<string>.Fail("Too many failed attempts. This code has been invalidated. Please request a new one.", 400);
            }

            var remainingAttempts = 5 - latestOtp.AttemptCount;
            return RequestResponse<string>.Fail($"Invalid verification code. You have {remainingAttempts} attempt(s) remaining.", 400);
        }

        // 3. Staging Phase: Generate Reset Token & Single Atomic Commit
        var resetToken = Guid.NewGuid().ToString("N");
        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _mediator.Send(new StageResetTokenSubCommand(latestOtp, resetToken, expiresAt), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        return RequestResponse<string>.Ok(resetToken, "Verification code verified successfully.");
    }
}
