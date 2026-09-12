using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.RefreshToken.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class RevokeAllUserRefreshTokensSubCommandHandler : IRequestHandler<RevokeAllUserRefreshTokensSubCommand, bool>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepository;

    public RevokeAllUserRefreshTokensSubCommandHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(RevokeAllUserRefreshTokensSubCommand request, CancellationToken cancellationToken)
    {
        var activeTokens = await _refreshTokenRepository
            .Get(t => t.UserId == request.UserId && !t.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
            token.UpdatedAt = DateTime.UtcNow;
            _refreshTokenRepository.Update(token);
        }

        return true;
    }
}
