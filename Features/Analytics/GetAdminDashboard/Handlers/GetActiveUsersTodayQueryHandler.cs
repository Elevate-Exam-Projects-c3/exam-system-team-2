using exam_system.Domain.Entities.Identity;
using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetAdminDashboard.Handlers;

public class GetActiveUsersTodayQueryHandler : IRequestHandler<GetActiveUsersTodayQuery, int>
{
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    public GetActiveUsersTodayQueryHandler(IGenericRepository<RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<int> Handle(GetActiveUsersTodayQuery request, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepository
            .Get(t => !t.IsDeleted && (t.CreatedAt >= request.TodayStartUtc || (t.UpdatedAt.HasValue && t.UpdatedAt.Value >= request.TodayStartUtc)))
            .Select(t => t.UserId)
            .Distinct()
            .CountAsync(cancellationToken);
    }
}
