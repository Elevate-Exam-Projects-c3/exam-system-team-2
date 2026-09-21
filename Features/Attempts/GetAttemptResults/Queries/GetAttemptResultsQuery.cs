using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptResults.Queries
{
    public record GetAttemptResultsQuery(Guid AttemptId, Guid CallerUserId) : IRequest<RequestResponse<AttemptResultsDto>>;
}
