using exam_system.Common.Enums;
using exam_system.Features.Identity.RefreshToken.Commands;
using exam_system.Features.Identity.RefreshToken.Dtos;
using exam_system.Features.Identity.RefreshToken.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Orchestrators;

public class RefreshTokenOrchestratorHandler : IRequestHandler<RefreshTokenOrchestratorRequest, RequestResponse<RefreshTokenDto>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<RefreshTokenDto>> Handle(RefreshTokenOrchestratorRequest request, CancellationToken cancellationToken)
    {
        // ─── Phase 1: Precondition Validation (Sub-Queries) ───────────────
        var tokenEntity = await _mediator.Send(new GetRefreshTokenByTokenQuery(request.RefreshToken), cancellationToken);
        if (tokenEntity == null)
        {
            return RequestResponse<RefreshTokenDto>.Fail("Invalid refresh token.", 401);
        }

        if (tokenEntity.IsRevoked)
        {
            return RequestResponse<RefreshTokenDto>.Fail("Refresh token has been revoked.", 401);
        }

        // AC2: Reuse compromise signal detection
        if (tokenEntity.IsUsed)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _mediator.Send(new RevokeAllUserRefreshTokensSubCommand(tokenEntity.UserId), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                throw;
            }

            return RequestResponse<RefreshTokenDto>.Fail("Refresh token has already been used. Please log in again.", 401);
        }

        if (tokenEntity.ExpiresAt <= DateTime.UtcNow)
        {
            return RequestResponse<RefreshTokenDto>.Fail("Refresh token has expired. Please log in again.", 401);
        }

        var user = await _mediator.Send(new GetUserByIdForRefreshQuery(tokenEntity.UserId), cancellationToken);
        if (user == null || user.AccountStatus != AccountStatus.Active)
        {
            return RequestResponse<RefreshTokenDto>.Fail("User account is inactive or not found.", 401);
        }

        // ─── Phase 2: Token Generation ────────────────────────────────────
        var newRefreshTokenResult = await _mediator.Send(new GenerateNewRefreshTokenQuery(), cancellationToken);
        var newAccessToken = await _mediator.Send(new GenerateAccessTokenForRefreshQuery(user), cancellationToken);

        // ─── Phase 3: Single Atomic Database Commit ───────────────────────
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _mediator.Send(new RotateRefreshTokenSubCommand(tokenEntity, newRefreshTokenResult.Token, newRefreshTokenResult.ExpiresAt), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        // ─── Phase 4: External Side-Effect (Outside Transaction) ───────────
        await _mediator.Send(new SetRefreshTokenCookieSubCommand(newRefreshTokenResult.Token, newRefreshTokenResult.ExpiresAt), cancellationToken);

        // ─── Phase 5: Return DTO ──────────────────────────────────────────
        var dto = new RefreshTokenDto(newAccessToken, "Bearer", 900);
        return RequestResponse<RefreshTokenDto>.Ok(dto, "Token refreshed successfully.");
    }
}
