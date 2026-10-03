namespace exam_system.Features.Diplomas.GetStudentDashboard.ViewModels
{
    public class StudentDashboardStatisticsViewModel
    {
        public double AverageScore { get; set; }
        public double PassRate { get; set; }
        public double TimeSpentInMinutes { get; set; }
        public int CorrectAnswersCount { get; set; }
    }
}
