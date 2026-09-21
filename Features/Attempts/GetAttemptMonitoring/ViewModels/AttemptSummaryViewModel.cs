using exam_system.Common.Enums;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.ViewModels
{
    public class AttemptSummaryViewModel
    {
        public Guid AttemptId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string QuizTitle { get; set; } = string.Empty;
        public AttemptStatus Status { get; set; }
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
