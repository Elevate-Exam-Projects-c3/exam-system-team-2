using System.ComponentModel.DataAnnotations;

namespace exam_system.Features.Diplomas.BrowseDiplomas
{
    public class DiplomaDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int CountOfQuizzes { get; set; }

    }
}
