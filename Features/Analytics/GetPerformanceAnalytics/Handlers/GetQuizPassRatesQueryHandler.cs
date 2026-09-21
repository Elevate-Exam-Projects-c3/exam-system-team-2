using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Handlers;

public class GetQuizPassRatesQueryHandler : IRequestHandler<GetQuizPassRatesQuery, IReadOnlyList<QuizPassRateDto>>
{
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;

    public GetQuizPassRatesQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<QuizPassRateDto>> Handle(GetQuizPassRatesQuery request, CancellationToken cancellationToken)
    {
        var query = _attemptRepository.Get(a => !a.IsDeleted && !a.Quiz.IsDeleted);

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
            .GroupBy(a => new { a.QuizId, a.Quiz.Title })
            .Select(g => new
            {
                QuizId = g.Key.QuizId,
                QuizTitle = g.Key.Title,
                TotalAttempts = g.Count(),
                PassedAttempts = g.Count(a => a.Passed == true)
            })
            .ToListAsync(cancellationToken);

        var result = new List<QuizPassRateDto>();

        foreach (var item in grouped)
        {
            double passRate = 0.0;
            if (item.TotalAttempts > 0)
            {
                passRate = Math.Round((double)item.PassedAttempts / item.TotalAttempts * 100.0, 2);
            }

            result.Add(new QuizPassRateDto(
                item.QuizId,
                item.QuizTitle,
                item.PassedAttempts,
                item.TotalAttempts,
                passRate
            ));
        }

        return result;
    }
}
