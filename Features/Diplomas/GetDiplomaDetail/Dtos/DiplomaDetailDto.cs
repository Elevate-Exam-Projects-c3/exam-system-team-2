namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos
{
    public class DiplomaDetailDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

    }
}
