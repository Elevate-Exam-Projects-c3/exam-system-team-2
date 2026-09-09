namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos
{
    public class QuizPublishReadinessDto
    {
        public Guid QuizId { get; set; }
        public bool IsReadyToPublish { get; set; }
        public List<PublishCheckItemDto> Checks { get; set; } = new();
    }
}
