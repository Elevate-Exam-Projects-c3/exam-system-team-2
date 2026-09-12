namespace exam_system.Features.Diplomas.GetDiplomaForEnrollment.Dtos
{
    public class DiplomaForEnrollmentDto
    {
        public Guid DiplomaId { get; set; }
        public bool IsDeleted { get; set; }
        public bool HasPublishedQuiz { get; set; } 
    }
}
