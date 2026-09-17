using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Attempts.GetAttemptMonitoring.ViewModels;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Mappings
{
    public static class AttemptDetailForAdminMappingExtensions
    {
        public static AttemptDetailForAdminViewModel ToViewModel(this AttemptDetailForAdminDto dto)
        {
            return new AttemptDetailForAdminViewModel
            {
                Score = dto.Score,
                Passed = dto.Passed,

                Questions = dto.Questions
                    .Select(q => new AdminQuestionResultViewModel
                    {
                        QuestionText = q.QuestionText,
                        Explanation = q.Explanation,
                        SelectedOptionText = q.SelectedOptionText,
                        IsCorrect = q.IsCorrect,
                        CorrectOptionText = q.CorrectOptionText
                    })
                    .ToList()
            };
        }
    }
}
