using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Attempts.GetAttemptResults.ViewModels;

namespace exam_system.Features.Attempts.GetAttemptResults.Mappings
{
    public static class AttemptResultsMappingExtensions
    {
        public static AttemptResultsViewModel ToViewModel(
            this AttemptResultsDto dto)
        {
            return new AttemptResultsViewModel
            {
                Score = dto.Score,
                Passed = dto.Passed,
                Questions = dto.Questions
                    .Select(q => new QuestionResultViewModel
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

