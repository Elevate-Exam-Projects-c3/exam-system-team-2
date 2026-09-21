namespace exam_system.Features.Attempts.GetAttemptMonitoring.ViewModels
{
    public class AttemptDetailForAdminViewModel
    {
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public List<AdminQuestionResultViewModel> Questions { get; set; } = new();
    }
}
