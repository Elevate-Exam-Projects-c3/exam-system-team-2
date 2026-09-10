namespace exam_system.Features.Quizzes.AdminPublishQuiz.ViewModels
{
    public class PublishQuizResultViewModel
    {
        public bool IsReadyToPublish { get; set; }
        public List<PublishQuizCheckItemViewModel> Checks { get; set; } = new();
    }
}
