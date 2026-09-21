using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Handlers;

public class GetDiplomaAverageScoresQueryHandler : IRequestHandler<GetDiplomaAverageScoresQuery, IReadOnlyList<DiplomaAverageScoreDto>>
{
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;

    public GetDiplomaAverageScoresQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<DiplomaAverageScoreDto>> Handle(GetDiplomaAverageScoresQuery request, CancellationToken cancellationToken)
    {
        var query = _attemptRepository.Get(a => !a.IsDeleted && !a.Quiz.IsDeleted && !a.Quiz.Diploma.IsDeleted && a.Score.HasValue);

        if (request.DiplomaId.HasValue)
        {
            var diplomaId = request.DiplomaId.Value;
            query = query.Where(a => a.Quiz.DiplomaId == diplomaId);
        }

        if (request.StartDate.HasValue)
        {
            var startDate = request.StartDate.Value;
            query = query.Where(a => a.CreatedAt >= startDate);
        }

        if (request.EndDate.HasValue)
        {
            var endDate = request.EndDate.Value;
            query = query.Where(a => a.CreatedAt <= endDate);
        }

        var grouped = await query
            .GroupBy(a => new { a.Quiz.DiplomaId, a.Quiz.Diploma.Title })
            .Select(g => new
            {
                DiplomaId = g.Key.DiplomaId,
                DiplomaTitle = g.Key.Title,
                TotalAttempts = g.Count(),
                AverageScore = g.Average(a => a.Score!.Value)
            })
            .ToListAsync(cancellationToken);

        var result = new List<DiplomaAverageScoreDto>();

        foreach (var item in grouped)
        {
            double averageScore = Math.Round(item.AverageScore, 2);

            result.Add(new DiplomaAverageScoreDto(
                item.DiplomaId,
                item.DiplomaTitle,
                averageScore,
                item.TotalAttempts
            ));
        }

        return result;
    }
}
