namespace exam_system.Features.Attempts.GetAttemptResults.ViewModels
{
    public class QuestionResultViewModel
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? Explanation { get; set; }
        public string? SelectedOptionText { get; set; }
        public bool IsCorrect { get; set; }
        public string? CorrectOptionText { get; set; }
    }
}
