using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators
{
    public record UnpublishQuizOrchestrator(Guid QuizId) : IRequest<RequestResponse>;
}
