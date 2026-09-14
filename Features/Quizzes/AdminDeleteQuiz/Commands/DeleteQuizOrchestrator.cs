using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Commands
{
    public record DeleteQuizOrchestrator(Guid QuizId) : IRequest<ApiResponse<bool>>;
}
