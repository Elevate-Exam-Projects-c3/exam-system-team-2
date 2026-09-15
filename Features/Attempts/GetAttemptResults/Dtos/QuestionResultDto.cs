namespace exam_system.Features.Attempts.GetAttemptResults.Dtos
{
    //Every question is in the result:
    public class QuestionResultDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? Explanation { get; set; }
        public string? SelectedOptionText { get; set; } // It is null if the student skipped the question.  
        public bool IsCorrect { get; set; }
        public string? CorrectOptionText { get; set; }
    }
}
