namespace exam_system.Features.Quizzes.AdminManageQuestions.ViewModels
{
    public class AddQuestionWithOptionsViewModel
    {
        public Guid QuizId { get; set; }

        public string Text { get; set; } = string.Empty;

        public string? Explanation { get; set; }

        public List<OptionViewModel> Options { get; set; } = [];
    }

    public class OptionViewModel
    {
        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
