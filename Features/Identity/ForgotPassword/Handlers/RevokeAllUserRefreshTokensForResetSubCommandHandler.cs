using MediatR;
using Microsoft.EntityFrameworkCore;
using RefreshTokenEntity = exam_system.Domain.Entities.Identity.RefreshToken;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class RevokeAllUserRefreshTokensForResetSubCommandHandler : IRequestHandler<RevokeAllUserRefreshTokensForResetSubCommand, bool>
{
    private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;

    public RevokeAllUserRefreshTokensForResetSubCommandHandler(IGenericRepository<RefreshTokenEntity> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(RevokeAllUserRefreshTokensForResetSubCommand request, CancellationToken cancellationToken)
    {
        var activeTokens = await _refreshTokenRepository.GetAll()
            .Where(t => t.UserId == request.UserId && !t.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
            _refreshTokenRepository.Update(token);
        }

        return true;
    }
}
