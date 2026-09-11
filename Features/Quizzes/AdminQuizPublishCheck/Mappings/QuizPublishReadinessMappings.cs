using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Mappings
{
    public static class QuizPublishReadinessMappings
    {

        //Mapping from QuizPublishReadinessDto to QuizPublishReadinessViewModel :
        public static QuizPublishReadinessViewModel ToViewModel(this QuizPublishReadinessDto dto)
        {
            return new QuizPublishReadinessViewModel
            {
                QuizId = dto.QuizId,
                IsReadyToPublish = dto.IsReadyToPublish,
                Checks = dto.Checks.Select(c => new PublishCheckItemViewModel
                {
                    CheckName = c.CheckName,
                    Passed = c.Passed,
                    Message = c.Message
                }).ToList()
            };
        }
    }
}
