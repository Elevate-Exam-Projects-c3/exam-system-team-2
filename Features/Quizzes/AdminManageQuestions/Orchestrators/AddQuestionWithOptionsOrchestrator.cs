using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using exam_system.Features.Quizzes.AdminManageQuestions.ViewModels;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators
{
    public record AddQuestionWithOptionsOrchestrator(
        Guid QuizId,
        string Text,
        string? Explanation,
        List<OptionItem> Options
    ) : IRequest<RequestResponse<Guid>>;

}
