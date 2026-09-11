using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.ViewModels;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Mappings
{
    public static class MappingExtensions
    {
        public static PublishQuizCheckItemViewModel ToViewModel(this PublishCheckItemDto dto)
        {
            return new PublishQuizCheckItemViewModel
            {
                CheckName = dto.CheckName,
                Passed = dto.Passed,
                Message = dto.Message
            };
        }

        public static PublishQuizResultViewModel ToViewModel(this PublishQuizResult result)
        {
            return new PublishQuizResultViewModel
            {
                IsReadyToPublish = result.IsReadyToPublish,
                Checks = result.Checks.Select(c => c.ToViewModel()).ToList()
            };
        }
    }
}

