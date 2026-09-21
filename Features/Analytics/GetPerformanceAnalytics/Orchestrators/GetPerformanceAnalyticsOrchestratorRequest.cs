using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Orchestrators;

public record GetPerformanceAnalyticsOrchestratorRequest(
    Guid? DiplomaId,
    DateTime? StartDate,
    DateTime? EndDate
) : IRequest<RequestResponse<PerformanceAnalyticsResponseViewModel>>;
