using System.ComponentModel.DataAnnotations;

namespace exam_system.Features.Diplomas.AdminCreateDiploma
{
    public class DiplomaDto
    {
        public string Title { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int CountOfQuizzes { get; set; }

    }
}
