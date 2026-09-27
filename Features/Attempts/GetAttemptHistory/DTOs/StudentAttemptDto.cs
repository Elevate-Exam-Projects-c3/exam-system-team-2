using exam_system.Common.Enums;

namespace exam_system.Features.Attempts.GetAttemptHistory.DTOs
{
    public class StudentAttemptDto
    {
        public required string QuizTitle { get; set; }
        public AttemptStatus Status { get; set; } 
        public double? Score { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int QuizQuestionsCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int QuizDurationMinutes { get; set; }
        public int? AttemptTakeTimeInMin{ get; set; }



    }
}
