using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

using RefreshToken = exam_system.Domain.Entities.Identity.RefreshToken;

public class CreateRefreshTokenSubCommandHandler : IRequestHandler<CreateRefreshTokenSubCommand, bool>
{
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    public CreateRefreshTokenSubCommandHandler(IGenericRepository<RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(CreateRefreshTokenSubCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Token = request.Token,
            ExpiresAt = request.ExpiresAt,
            IsUsed = false,
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        return true;
    }
}
