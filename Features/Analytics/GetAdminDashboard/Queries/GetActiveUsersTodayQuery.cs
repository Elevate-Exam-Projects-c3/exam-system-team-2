using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Queries;

public record GetActiveUsersTodayQuery(DateTime TodayStartUtc) : IRequest<int>;
