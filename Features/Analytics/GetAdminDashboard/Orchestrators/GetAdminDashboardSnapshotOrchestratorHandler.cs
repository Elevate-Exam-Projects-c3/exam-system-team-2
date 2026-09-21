using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Features.Analytics.GetAdminDashboard.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace exam_system.Features.Analytics.GetAdminDashboard.Orchestrators;

public class GetAdminDashboardSnapshotOrchestratorHandler : IRequestHandler<GetAdminDashboardSnapshotOrchestratorRequest, RequestResponse<AdminDashboardSnapshotViewModel>>
{
    private const string CacheKey = "AdminDashboardSnapshot";
    private readonly IMediator _mediator;
    private readonly IMemoryCache _memoryCache;

    public GetAdminDashboardSnapshotOrchestratorHandler(
        IMediator mediator,
        IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    public async Task<RequestResponse<AdminDashboardSnapshotViewModel>> Handle(GetAdminDashboardSnapshotOrchestratorRequest request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(CacheKey, out AdminDashboardSnapshotViewModel? cachedViewModel))
        {
            if (cachedViewModel != null)
            {
                return RequestResponse<AdminDashboardSnapshotViewModel>.Ok(cachedViewModel, "Dashboard snapshot retrieved from cache.");
            }
        }

        var todayStart = DateTime.UtcNow.Date;

        var totalRegisteredUsers = await _mediator.Send(new GetTotalRegisteredUsersQuery(), cancellationToken);
        var activeUsersToday = await _mediator.Send(new GetActiveUsersTodayQuery(todayStart), cancellationToken);
        var totalDiplomas = await _mediator.Send(new GetTotalDiplomasCountQuery(), cancellationToken);
        var totalQuizzes = await _mediator.Send(new GetTotalQuizzesCountQuery(), cancellationToken);
        var attemptsStats = await _mediator.Send(new GetQuizAttemptsStatsQuery(), cancellationToken);

        var viewModel = new AdminDashboardSnapshotViewModel(
            TotalRegisteredUsers: totalRegisteredUsers,
            ActiveUsersToday: activeUsersToday,
            TotalDiplomas: totalDiplomas,
            TotalQuizzes: totalQuizzes,
            TotalAttempts: attemptsStats.TotalAttempts,
            OverallAveragePassRate: attemptsStats.PassRate,
            SnapshotGeneratedAtUtc: DateTime.UtcNow
        );

        _memoryCache.Set(CacheKey, viewModel, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return RequestResponse<AdminDashboardSnapshotViewModel>.Ok(viewModel, "Dashboard snapshot retrieved successfully.");
    }
}
