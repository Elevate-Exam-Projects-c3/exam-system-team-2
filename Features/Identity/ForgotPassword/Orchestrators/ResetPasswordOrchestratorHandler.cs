using MediatR;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public class ResetPasswordOrchestratorHandler : IRequestHandler<ResetPasswordOrchestratorRequest, RequestResponse<bool>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<bool>> Handle(ResetPasswordOrchestratorRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        // 1. Precondition Sub-Query: Validate Reset Token
        var otp = await _mediator.Send(new GetPasswordResetOtpByTokenQuery(normalizedEmail, request.ResetToken), cancellationToken);
        if (otp == null || otp.ResetToken != request.ResetToken || otp.IsUsed)
        {
            return RequestResponse<bool>.Fail("Invalid or expired reset token.", 400);
        }

        if (otp.ResetTokenExpiresAt.HasValue && DateTime.UtcNow > otp.ResetTokenExpiresAt.Value)
        {
            return RequestResponse<bool>.Fail("Reset token has expired. Please request a new password reset.", 400);
        }

        // 2. Precondition Sub-Query: Validate User
        var user = await _mediator.Send(new GetUserByEmailForForgotPasswordQuery(normalizedEmail), cancellationToken);
        if (user == null)
        {
            return RequestResponse<bool>.Fail("User not found.", 404);
        }

        // 3. Staging Phase & Single Atomic Database Commit
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Stage password change & reset lockout/failed attempts
            await _mediator.Send(new UpdateUserPasswordSubCommand(user, request.NewPassword), cancellationToken);

            // Stage OTP consumption
            await _mediator.Send(new ConsumePasswordResetOtpSubCommand(otp), cancellationToken);

            // Stage revocation of all active refresh tokens (force re-login on all devices)
            await _mediator.Send(new RevokeAllUserRefreshTokensForResetSubCommand(user.Id), cancellationToken);

            // Atomic database commit
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        return RequestResponse<bool>.Ok(true, "Password has been reset successfully. Please log in with your new password.");
    }
}
