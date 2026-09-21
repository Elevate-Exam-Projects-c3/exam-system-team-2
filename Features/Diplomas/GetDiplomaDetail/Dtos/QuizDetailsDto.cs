namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos
{
    public class QuizDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; }
        public int? MaxAttempts { get; set; }
    }
}
