using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using MediatR;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;

public record GetQuizPassRatesQuery(
    Guid? DiplomaId,
    DateTime? StartDate,
    DateTime? EndDate
) : IRequest<IReadOnlyList<QuizPassRateDto>>;
