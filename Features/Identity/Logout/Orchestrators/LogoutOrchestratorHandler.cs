using exam_system.Features.Identity.Logout.Commands;
using exam_system.Features.Identity.Logout.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Logout.Orchestrators;

public class LogoutOrchestratorHandler : IRequestHandler<LogoutOrchestratorRequest, RequestResponse<bool>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<bool>> Handle(LogoutOrchestratorRequest request, CancellationToken cancellationToken)
    {
        // ─── Phase 1: Revoke token in DB if token provided ────────────────
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var tokenEntity = await _mediator.Send(new GetRefreshTokenForLogoutQuery(request.RefreshToken), cancellationToken);
            if (tokenEntity != null && !tokenEntity.IsRevoked)
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    await _mediator.Send(new RevokeRefreshTokenSubCommand(tokenEntity), cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                    throw;
                }
            }
        }

        // ─── Phase 2: Clear cookie client-side (Side-Effect) ──────────────
        await _mediator.Send(new ClearRefreshTokenCookieSubCommand(), cancellationToken);

        // ─── Phase 3: Return Primitive Boolean ────────────────────────────
        return RequestResponse<bool>.Ok(true, "Logged out successfully.");
    }
}
