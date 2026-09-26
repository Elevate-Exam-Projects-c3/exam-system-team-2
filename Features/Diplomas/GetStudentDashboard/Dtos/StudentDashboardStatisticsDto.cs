namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos
{
    public class StudentDashboardStatisticsDto
    {
        public double AverageScore { get; set; }
        public double PassRate { get; set; }
        public double TimeSpentInMinutes { get; set; }
        public int CorrectAnswersCount { get; set; }

    }
}
