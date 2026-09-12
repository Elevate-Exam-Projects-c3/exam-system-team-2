using exam_system.Domain.Common;
using exam_system.Domain.Entities.Quizzes;
using System.ComponentModel.DataAnnotations;

namespace exam_system.Domain.Entities.Diplomas;

public class Diploma : BaseEntity
{
    [StringLength(200,MinimumLength = 3, ErrorMessage = "The title must be between 3 and 200 characters.")]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000, ErrorMessage = "The description cannot be longer than 1000 characters.")]
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Navigations
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
}
