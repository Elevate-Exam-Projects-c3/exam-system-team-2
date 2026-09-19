using exam_system.Common.Enums;
using exam_system.Features.Attempts.StartAttempt.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries
{
    public record GetExistStudentAttemptQuery(Guid StudentId, Guid QuizId) : IRequest<RequestResponse<AttemptStatusDto>>;
}
