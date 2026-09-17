namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels
{
    public class QuizPublishReadinessViewModel
    {
        public Guid QuizId { get; set; }
        public bool IsReadyToPublish { get; set; }
        public List<PublishCheckItemViewModel> Checks { get; set; } = new();
    }
}
