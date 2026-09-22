using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Attempts.GetAttemptMonitoring.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Mappings
{
    public static class AttemptSummaryMappingExtensions
    {
        // Converts an AttemptSummaryDto to an AttemptSummaryViewModel :
        public static AttemptSummaryViewModel ToViewModel(this AttemptSummaryDto dto)
        {
            return new AttemptSummaryViewModel
            {
                AttemptId = dto.AttemptId,
                StudentName = dto.StudentName,
                QuizTitle = dto.QuizTitle,
                Status = dto.Status,
                Score = dto.Score,
                Passed = dto.Passed,
                SubmittedAt = dto.SubmittedAt
            };
        }

        // Converts a PaginatedResult of AttemptSummaryDto to a PaginatedResult of AttemptSummaryViewModel :
        public static PaginatedResult<AttemptSummaryViewModel> ToViewModel(this PaginatedResult<AttemptSummaryDto> result)
        {
            return PaginatedResult<AttemptSummaryViewModel>.Create(
                result.Items
                    .Select(x => x.ToViewModel())
                    .ToList(),
                result.TotalCount,
                result.PageIndex,
                result.PageSize);
        }
    }
}
