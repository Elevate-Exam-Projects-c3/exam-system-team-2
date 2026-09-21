namespace exam_system.Features.Quizzes.AdminPublishQuiz.ViewModels
{
    public class PublishQuizCheckItemViewModel
    {
        public string CheckName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string? Message { get; set; }
    }
}
