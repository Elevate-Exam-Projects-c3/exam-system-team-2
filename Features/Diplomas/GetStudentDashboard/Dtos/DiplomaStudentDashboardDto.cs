namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos
{
    public class DiplomaStudentDashboardDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public int TotalQuizzesCount { get; set; }
        public int TakenQuizzesCount { get; set; }
    }
}
