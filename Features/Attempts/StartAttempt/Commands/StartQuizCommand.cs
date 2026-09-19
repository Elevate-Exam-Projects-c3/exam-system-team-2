using exam_system.Features.Attempts.StartAttempt.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Commands
{
    public record StartAttemptCommand(Guid QuizId, Guid StudentId) : IRequest<ApiResponse<bool>>;
}
