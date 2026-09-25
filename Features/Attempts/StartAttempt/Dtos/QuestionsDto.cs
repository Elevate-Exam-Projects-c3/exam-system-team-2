namespace exam_system.Features.Attempts.StartAttempt.Dtos
{
    public class QuestionsDto
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public ICollection<AttemptOptionDto> Options { get; set; } = new List<AttemptOptionDto>();
    }

    public class AttemptOptionDto
    {
        public Guid Id { get; set; } 
        public string OptionText { get; set; } = string.Empty;
    }
}
