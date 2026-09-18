namespace exam_system.Features.Attempts.StartAttempt.Dtos
{
    public class StartAttemptDto
    {
        public Guid AttemptId;
        public DateTime Deadline;
        public ICollection<AttemptOptionDto> Options;
    }
    public class AttemptOptionDto
    {
        public Guid OptionId;
        public string OptionText;
    }
}
