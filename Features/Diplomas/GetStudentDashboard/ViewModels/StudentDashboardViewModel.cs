namespace exam_system.Features.Diplomas.GetStudentDashboard.ViewModels
{
    public class StudentDashboardViewModel
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }

        public int TotalQuizzesCount { get; set; }

        public StudentDashboardStatisticsViewModel Statistics { get; set; } = null!;

        public ICollection<DiplomaStudentDashboardViewModel> Diplomas { get; set; }
            = new List<DiplomaStudentDashboardViewModel>();

        public ICollection<RecentQuizAttemptViewModel> RecentAttempts { get; set; }
            = new List<RecentQuizAttemptViewModel>();
    }
}
