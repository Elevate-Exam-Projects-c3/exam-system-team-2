using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.RefreshToken.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class RotateRefreshTokenSubCommandHandler : IRequestHandler<RotateRefreshTokenSubCommand, bool>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepository;

    public RotateRefreshTokenSubCommandHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(RotateRefreshTokenSubCommand request, CancellationToken cancellationToken)
    {
        request.OldToken.IsUsed = true;
        request.OldToken.ReplacedByToken = request.NewToken;
        request.OldToken.UpdatedAt = DateTime.UtcNow;
        _refreshTokenRepository.Update(request.OldToken);

        var newRefreshToken = new Domain.Entities.Identity.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = request.OldToken.UserId,
            Token = request.NewToken,
            ExpiresAt = request.ExpiresAt,
            IsUsed = false,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken);
        return true;
    }
}
