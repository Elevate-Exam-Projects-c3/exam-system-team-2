using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.RefreshToken.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class GetRefreshTokenByTokenQueryHandler : IRequestHandler<GetRefreshTokenByTokenQuery, Domain.Entities.Identity.RefreshToken?>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepository;

    public GetRefreshTokenByTokenQueryHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Domain.Entities.Identity.RefreshToken?> Handle(GetRefreshTokenByTokenQuery request, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepository
            .Get(t => t.Token == request.Token)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
