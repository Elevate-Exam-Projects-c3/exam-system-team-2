using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators
{
    public record PublishQuizOrchestrator(Guid QuizId): IRequest<RequestResponse<PublishQuizResult>>;
}
