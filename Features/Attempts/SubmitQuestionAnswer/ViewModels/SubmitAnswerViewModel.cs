namespace exam_system.Features.Attempts.SubmitQuestionAnswer.ViewModels
{
    public class SubmitAnswerViewModel
    {
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
    }
}
