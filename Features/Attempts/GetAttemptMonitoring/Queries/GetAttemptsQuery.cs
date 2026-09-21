using exam_system.Common.Enums;
using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Queries
{
    public record GetAttemptsQuery(
    Guid? QuizId,
    Guid? StudentId,
    AttemptStatus? Status,
    AttemptSortOrder SortOrder = AttemptSortOrder.Descending,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<RequestResponse<PaginatedResult<AttemptSummaryDto>>>;
}
