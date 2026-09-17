using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Logout.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.Logout.Handlers;

public class GetRefreshTokenForLogoutQueryHandler : IRequestHandler<GetRefreshTokenForLogoutQuery, Domain.Entities.Identity.RefreshToken?>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepository;

    public GetRefreshTokenForLogoutQueryHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Domain.Entities.Identity.RefreshToken?> Handle(GetRefreshTokenForLogoutQuery request, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepository
            .Get(t => t.Token == request.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
