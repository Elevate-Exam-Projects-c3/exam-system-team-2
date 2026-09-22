namespace exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels
{
    public class ViewDiplomaDetailsViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public List<DiplomaQuizDetailsViewModel> Quizzes { get; set; } = [];
    }
}
