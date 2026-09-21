using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries
{
    public record GetChickRightToAttemptQuery(Guid QuizId, Guid StudentId) : IRequest<int>;
}
