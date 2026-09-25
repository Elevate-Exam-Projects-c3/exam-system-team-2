namespace exam_system.Features.Attempts.StartAttempt.Dtos
{
    public class StartAttemptResponseDto
    {
        public Guid AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<QuestionsDto> Questions { get; set; }
    }
}
