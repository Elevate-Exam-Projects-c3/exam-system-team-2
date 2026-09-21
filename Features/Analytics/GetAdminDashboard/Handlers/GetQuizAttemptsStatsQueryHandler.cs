using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetAdminDashboard.Handlers;

public class GetQuizAttemptsStatsQueryHandler : IRequestHandler<GetQuizAttemptsStatsQuery, QuizAttemptsStatsDto>
{
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;

    public GetQuizAttemptsStatsQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<QuizAttemptsStatsDto> Handle(GetQuizAttemptsStatsQuery request, CancellationToken cancellationToken)
    {
        var totalAttempts = await _attemptRepository.CountAsync(a => !a.IsDeleted);

        var stats = await _attemptRepository
            .Get(a => !a.IsDeleted && a.Passed.HasValue)
            .GroupBy(_ => true)
            .Select(g => new
            {
                FinishedAttempts = g.Count(),
                PassedAttempts = g.Count(a => a.Passed == true)
            })
            .FirstOrDefaultAsync(cancellationToken);

        int finishedAttempts = 0;
        int passedAttempts = 0;

        if (stats != null)
        {
            finishedAttempts = stats.FinishedAttempts;
            passedAttempts = stats.PassedAttempts;
        }

        double passRate = 0.0;
        if (finishedAttempts > 0)
        {
            passRate = Math.Round((double)passedAttempts / finishedAttempts * 100.0, 2);
        }

        return new QuizAttemptsStatsDto(totalAttempts, passRate);
    }
}
