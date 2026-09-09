using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminManageQuestions.ViewModels;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Mappings
{
    public static class MappingExtensions   
    {

        // Mapping method to convert AddQuestionWithOptionsViewModel to AddQuestionWithOptionsOrchestrator
        public static AddQuestionWithOptionsOrchestrator ToOrchestrator(
            this AddQuestionWithOptionsViewModel request)
        {
            return new AddQuestionWithOptionsOrchestrator(
                request.QuizId,
                request.Text,
                request.Explanation,
                request.Options.Select(o => new OptionItem(o.OptionText, o.IsCorrect)).ToList());
        }
    }
}

