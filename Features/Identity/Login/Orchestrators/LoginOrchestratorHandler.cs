using exam_system.Common.Enums;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Identity.Login.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Login.Orchestrators;

public class LoginOrchestratorHandler : IRequestHandler<LoginOrchestratorRequest, RequestResponse<string>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<string>> Handle(LoginOrchestratorRequest request, CancellationToken cancellationToken)
    {
        // ─── Phase 1: Precondition Validation (Sub-Queries) ───────────────
        var user = await _mediator.Send(new GetUserByEmailForLoginQuery(request.Email), cancellationToken);
        if (user == null)
        {
            return RequestResponse<string>.Fail("Invalid email or password.", 400);
        }

        // AC4: Lockout check (consecutive failed attempts lockout window)
        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
        {
            return RequestResponse<string>.Fail("Account is locked due to multiple failed login attempts. Please try again later.", 400);
        }

        // AC1: Requires AccountStatus=active and EmailConfirmed=true; pending rejected with explanatory error
        if (!user.EmailConfirmed || user.AccountStatus == AccountStatus.Pending)
        {
            return RequestResponse<string>.Fail("Account is pending verification. Please verify your email before logging in.", 400);
        }

        if (user.AccountStatus != AccountStatus.Active)
        {
            return RequestResponse<string>.Fail("Account is not active.", 400);
        }

        // ─── Phase 2: Password Verification ───────────────────────────────
        var isPasswordValid = await _mediator.Send(new ValidatePasswordQuery(request.Password, user.PasswordHash), cancellationToken);
        if (!isPasswordValid)
        {
            // Record failed attempt in an atomic transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _mediator.Send(new RecordFailedLoginAttemptSubCommand(user), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                throw;
            }

            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return RequestResponse<string>.Fail("Account is locked due to multiple failed login attempts. Please try again later.", 400);
            }

            return RequestResponse<string>.Fail("Invalid email or password.", 400);
        }

        // ─── Phase 3: Token Generation ────────────────────────────────────
        var accessToken = await _mediator.Send(new GenerateAccessTokenQuery(user), cancellationToken);
        var refreshTokenResult = await _mediator.Send(new GenerateRefreshTokenQuery(), cancellationToken);

        // ─── Phase 4: Single Atomic Database Commit ───────────────────────
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Stage reset failed login attempts (ApplicationUser aggregate only)
            await _mediator.Send(new ResetFailedLoginAttemptsSubCommand(user), cancellationToken);

            // Stage refresh token (RefreshToken aggregate only)
            await _mediator.Send(new CreateRefreshTokenSubCommand(user.Id, refreshTokenResult.Token, refreshTokenResult.ExpiresAt), cancellationToken);

            // Single atomic commit
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        // ─── Phase 5: Side-Effect (Outside Transaction) ───────────────────
        await _mediator.Send(new SetRefreshTokenCookieSubCommand(refreshTokenResult.Token, refreshTokenResult.ExpiresAt), cancellationToken);

        // ─── Phase 6: Return Primitive JWT String ─────────────────────────
        return RequestResponse<string>.Ok(accessToken, "Login successful.");
    }
}
