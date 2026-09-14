using exam_system.Common.Enums;
using exam_system.Domain.Common;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Attempts;

namespace exam_system.Domain.Entities.Quizzes;

public class Quiz : BaseEntity
{
    public Guid DiplomaId { get; set; }
    public Diploma Diploma { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public int DurationMinutes { get; set; }
    public int Score { get; set; }
    public int PassScore { get; set; } 
    public int? MaxAttempts { get; set; } = 3;     
    public QuizStatus Status { get; set; } = QuizStatus.Draft;
    public DateTime? PublishedAt { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigations
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();

    public void UnPublichArchived()
    {
        if(Status == QuizStatus.Published)
        {
            Status = QuizStatus.Archived;
            UpdatedAt = DateTime.UtcNow;
        }
    }
    public void UnPublishDraft()
    {
        if (Status == QuizStatus.Published)
        {
            Status = QuizStatus.Draft;
        }
    }
    public void Publish()
    {
        if (Status == QuizStatus.Draft || Status == QuizStatus.Archived)
        {
            Status = QuizStatus.Published;
            PublishedAt = DateTime.UtcNow;
        }
    }
}
