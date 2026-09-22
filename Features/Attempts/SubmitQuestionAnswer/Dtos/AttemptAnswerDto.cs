using exam_system.Common.Enums;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Dtos
{
    public class AttemptAnswerDto
    {
        public Guid AttemptId { get; set; }
        public Guid QuizId { get; set; }
        public AttemptStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public Guid StudentUserId { get; set; }
    }
}
