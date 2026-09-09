using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

public class VerifyEmailOtpCommandHandler : IRequestHandler<VerifyEmailOtpCommand, RequestResponse<VerifyEmailOtpResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailOtpCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<VerifyEmailOtpResponse>> Handle(VerifyEmailOtpCommand request, CancellationToken cancellationToken)
    {
        // ─── Phase 1: Precondition Validation ─────────────────────────────
        var user = await _mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);
        if (user == null)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("User not found.", 404);
        }

        if (user.EmailConfirmed)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Email is already verified.", 400);
        }

        var latestOtp = await _mediator.Send(new GetLatestActiveOtpByEmailQuery(request.Email), cancellationToken);
        if (latestOtp == null)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("No active OTP found. Please request a new one.", 400);
        }

        // AC3: Locked check (5 or more incorrect attempts)
        if (latestOtp.AttemptCount >= 5)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Too many failed attempts. This OTP has been locked. Please request a new one.", 400);
        }

        // AC2: Expiry check (exact 10 minutes)
        if (DateTime.UtcNow > latestOtp.ExpiresAt)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("OTP code has expired. Please request a new one.", 400);
        }

        // ─── Phase 2: OTP Validation ──────────────────────────────────────
        var isMatch = await _mediator.Send(new ValidateOtpHashQuery(request.Otp, latestOtp.OtpHash), cancellationToken);

        if (!isMatch)
        {
            // Transaction to commit incremented attempt count atomically
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _mediator.Send(new IncrementOtpAttemptCountSubCommand(latestOtp), cancellationToken);
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
                return RequestResponse<VerifyEmailOtpResponse>.Fail("Too many failed attempts. This OTP has been locked. Please request a new one.", 400);
            }

            var remainingAttempts = 5 - latestOtp.AttemptCount;
            return RequestResponse<VerifyEmailOtpResponse>.Fail($"Invalid OTP. You have {remainingAttempts} attempt(s) remaining.", 400);
        }

        // ─── Phase 3: Single Atomic Database Commit ───────────────────────
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _mediator.Send(new ActivateUserAndConsumeOtpSubCommand(user, latestOtp), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        // ─── Phase 4: Success Response ────────────────────────────────────
        return RequestResponse<VerifyEmailOtpResponse>.Ok(
            new VerifyEmailOtpResponse(user.Id, user.Email, "Email verified successfully. Your account is now active."),
            "Email verified successfully.");
    }
}
