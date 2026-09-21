namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos
{
    public class ViewDiplomaDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public List<DiplomaQuizDetailsDto> Quizzes { get; set; } = new();
    }
}
