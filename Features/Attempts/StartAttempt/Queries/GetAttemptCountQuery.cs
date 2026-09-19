using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries
{
    public record GetAttemptCountQuery(Guid QuizId, Guid StudentId) : IRequest<int>;
}
