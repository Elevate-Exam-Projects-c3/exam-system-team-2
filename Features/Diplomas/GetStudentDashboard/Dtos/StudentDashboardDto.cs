namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos
{
    public class StudentDashboardDto
    {
        //Student information 
        public Guid Id { get; set; }
        public required string FullName { get; set; }

        public int TotalQuizzesCount { get; set; }

        public StudentDashboardStatisticsDto Statistics { get; set; } = null!;

        public ICollection<DiplomaStudentDashboardDto> Diplomas { get; set; }
            = new List<DiplomaStudentDashboardDto>();

        public ICollection<RecentQuizAttemptDto> RecentAttempts { get; set; }
            = new List<RecentQuizAttemptDto>();
    }
}
