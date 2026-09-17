using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using MediatR;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;

public record GetTopFailedQuestionsQuery(
    Guid? DiplomaId,
    DateTime? StartDate,
    DateTime? EndDate
) : IRequest<IReadOnlyList<FailedQuestionDto>>;
