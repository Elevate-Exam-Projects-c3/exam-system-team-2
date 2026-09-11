using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands
{
    public record UnpublishQuizCommand(Guid QuizId) : IRequest<RequestResponse>;
}
