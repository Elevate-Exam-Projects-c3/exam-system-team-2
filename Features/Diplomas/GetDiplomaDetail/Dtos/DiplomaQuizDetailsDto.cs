namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos
{
    public class DiplomaQuizDetailsDto
    {
        public QuizDetailsDto Quiz { get; set; } = null!;
        public StudentAttemptDto StudentAttempt { get; set; } = null!;
    }
}
