namespace exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels
{
    public class DiplomaDetailsViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public IEnumerable<QuizDetailsViewModel> Quizzes { get; set; } = [];
    }
}
