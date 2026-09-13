using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Logout.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Logout.Handlers;

public class RevokeRefreshTokenSubCommandHandler : IRequestHandler<RevokeRefreshTokenSubCommand, bool>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepository;

    public RevokeRefreshTokenSubCommandHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public Task<bool> Handle(RevokeRefreshTokenSubCommand request, CancellationToken cancellationToken)
    {
        request.Token.IsRevoked = true;
        request.Token.UpdatedAt = DateTime.UtcNow;
        _refreshTokenRepository.Update(request.Token);
        return Task.FromResult(true);
    }
}
