namespace exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels
{
    public class QuizDetailsViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; }
        public int? MaxAttempts { get; set; }
    }
}
