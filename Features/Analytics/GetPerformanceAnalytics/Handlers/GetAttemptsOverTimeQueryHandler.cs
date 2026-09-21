using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Handlers;

public class GetAttemptsOverTimeQueryHandler : IRequestHandler<GetAttemptsOverTimeQuery, IReadOnlyList<AttemptsOverTimeDto>>
{
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;

    public GetAttemptsOverTimeQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<AttemptsOverTimeDto>> Handle(GetAttemptsOverTimeQuery request, CancellationToken cancellationToken)
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
            .GroupBy(a => a.CreatedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        var result = new List<AttemptsOverTimeDto>();

        foreach (var item in grouped)
        {
            result.Add(new AttemptsOverTimeDto(
                item.Date.ToString("yyyy-MM-dd"),
                item.Count
            ));
        }

        return result;
    }
}
