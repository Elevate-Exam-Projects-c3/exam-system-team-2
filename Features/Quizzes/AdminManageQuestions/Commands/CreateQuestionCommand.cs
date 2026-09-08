using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands
{
    public record CreateQuestionCommand(Guid QuizId, string Text, string? Explanation) : IRequest<RequestResponse<Guid>>
    {
        

    }
}