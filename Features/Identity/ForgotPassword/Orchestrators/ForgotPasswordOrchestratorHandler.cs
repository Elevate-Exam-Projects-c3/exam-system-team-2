using MediatR;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public class ForgotPasswordOrchestratorHandler : IRequestHandler<ForgotPasswordOrchestratorRequest, RequestResponse<bool>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<bool>> Handle(ForgotPasswordOrchestratorRequest request, CancellationToken cancellationToken)
    {
        const string neutralMessage = "If this email is registered, a code has been sent.";
        var normalizedEmail = request.Email.Trim().ToLower();

        // 1. Precondition Sub-Query: Find User
        var user = await _mediator.Send(new GetUserByEmailForForgotPasswordQuery(normalizedEmail), cancellationToken);
        if (user == null)
        {
            // Prevent account enumeration by returning neutral response
            return RequestResponse<bool>.Ok(true, neutralMessage);
        }

        // 2. 30-Second Cooldown Check
        var latestOtp = await _mediator.Send(new GetLatestPasswordResetOtpQuery(normalizedEmail), cancellationToken);
        if (latestOtp != null && !latestOtp.IsUsed && DateTime.UtcNow < latestOtp.ExpiresAt)
        {
            if (DateTime.UtcNow - latestOtp.CreatedAt < TimeSpan.FromSeconds(30))
            {
                return RequestResponse<bool>.Fail("Please wait 30 seconds before requesting another code.", 400);
            }
        }

        // 3. Staging Phase & Single Atomic Database Commit
        string plainOtp;
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            plainOtp = await _mediator.Send(new StagePasswordResetOtpSubCommand(user.Id, user.Email), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        // 4. External Side-Effect: Dispatch Email Outside DB Transaction
        await _mediator.Send(new SendPasswordResetEmailSubCommand(user.Email, user.FullName, plainOtp), cancellationToken);

        // 5. Neutral Success Response
        return RequestResponse<bool>.Ok(true, neutralMessage);
    }
}
