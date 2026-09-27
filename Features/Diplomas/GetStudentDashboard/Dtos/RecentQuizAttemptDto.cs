namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos
{
    public class RecentQuizAttemptDto
    {
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;

        public double? Score { get; set; }
        public bool? Passed { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime SubmittedAt { get; set; }

        public int CorrectAnswersCount { get; set; }
    }
}
