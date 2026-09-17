namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels
{
    public class PublishCheckItemViewModel
    {
        public string CheckName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string? Message { get; set; }
    }
}
