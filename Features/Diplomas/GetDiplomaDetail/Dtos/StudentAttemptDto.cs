namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos
{
    public class StudentAttemptDto
    {
        public Guid QuizId { get; set; }
        public int AttemptCount { get; set; }
        public bool IsResumable { get; set; }
        public bool CanStudentAttempt { get; set; }
    }
}
