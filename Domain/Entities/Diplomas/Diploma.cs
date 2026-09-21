using exam_system.Domain.Common;
using exam_system.Domain.Entities.Quizzes;
using System.ComponentModel.DataAnnotations;

namespace exam_system.Domain.Entities.Diplomas;

public class Diploma : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Navigations
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
}
