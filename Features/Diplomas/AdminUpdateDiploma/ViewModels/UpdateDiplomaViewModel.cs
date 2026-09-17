using System.ComponentModel.DataAnnotations;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.ViewModels
{
    public class UpdateDiplomaViewModel
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
