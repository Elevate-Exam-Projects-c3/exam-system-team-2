namespace exam_system.Features.Attempts.GetAttemptResults.Dtos
{
    //The full result Attempt : 
    public class AttemptResultsDto
    {
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public List<QuestionResultDto> Questions { get; set; } = new();
    }
}
