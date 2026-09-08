using System.ComponentModel.DataAnnotations;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels
{
    public class DiplomaViewModel
    {
        [Range(3, 200, ErrorMessage = "The title must be between 3 and 200 characters.")]
        public string Title { get; set; } = string.Empty;
        [MaxLength(1000, ErrorMessage = "The description cannot be longer than 1000 characters.")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

    }
}
