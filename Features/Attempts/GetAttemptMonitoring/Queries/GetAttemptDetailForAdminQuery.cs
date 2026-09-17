using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Queries
{
    public record GetAttemptDetailForAdminQuery(Guid AttemptId) : IRequest<RequestResponse<AttemptDetailForAdminDto>>;
}
