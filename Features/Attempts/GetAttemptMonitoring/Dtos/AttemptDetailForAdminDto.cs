namespace exam_system.Features.Attempts.GetAttemptMonitoring.Dtos
{
    public class AttemptDetailForAdminDto
    {
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public List<AdminQuestionResultDto> Questions { get; set; } = new();
    }
}
