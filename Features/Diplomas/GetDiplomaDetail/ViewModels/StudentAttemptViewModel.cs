namespace exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels
{
    public class StudentAttemptViewModel
    {
        public Guid QuizId { get; set; }
        public int AttemptCount { get; set; }
        public bool IsResumable { get; set; }
        public bool CanStudentAttempt { get; set; }
    }
}
