using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries
{
    public record GetQuizPublishReadinessQuery(Guid QuizId) : IRequest<QuizPublishReadinessDto?>
    {
    }
}
