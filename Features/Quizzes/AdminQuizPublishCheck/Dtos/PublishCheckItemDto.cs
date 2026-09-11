namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos
{
    public class PublishCheckItemDto
    {
        public string CheckName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string? Message { get; set; }
    }
}
