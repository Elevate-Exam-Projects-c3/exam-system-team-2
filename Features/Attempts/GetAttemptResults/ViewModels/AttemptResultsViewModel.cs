namespace exam_system.Features.Attempts.GetAttemptResults.ViewModels
{
    public class AttemptResultsViewModel
    {
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public List<QuestionResultViewModel> Questions { get; set; } = new();
    }
}
