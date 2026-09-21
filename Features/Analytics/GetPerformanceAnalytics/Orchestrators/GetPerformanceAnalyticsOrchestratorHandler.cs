using exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Orchestrators;

public class GetPerformanceAnalyticsOrchestratorHandler : IRequestHandler<GetPerformanceAnalyticsOrchestratorRequest, RequestResponse<PerformanceAnalyticsResponseViewModel>>
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _memoryCache;

    public GetPerformanceAnalyticsOrchestratorHandler(
        IMediator mediator,
        IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    public async Task<RequestResponse<PerformanceAnalyticsResponseViewModel>> Handle(GetPerformanceAnalyticsOrchestratorRequest request, CancellationToken cancellationToken)
    {
        string diplomaPart = "all";
        if (request.DiplomaId.HasValue)
        {
            diplomaPart = request.DiplomaId.Value.ToString();
        }

        string startPart = "none";
        if (request.StartDate.HasValue)
        {
            startPart = request.StartDate.Value.ToString("yyyyMMdd");
        }

        string endPart = "none";
        if (request.EndDate.HasValue)
        {
            endPart = request.EndDate.Value.ToString("yyyyMMdd");
        }

        string cacheKey = "PerfAnalytics_" + diplomaPart + "_" + startPart + "_" + endPart;

        if (_memoryCache.TryGetValue(cacheKey, out PerformanceAnalyticsResponseViewModel? cachedViewModel))
        {
            if (cachedViewModel != null)
            {
                return RequestResponse<PerformanceAnalyticsResponseViewModel>.Ok(cachedViewModel, "Performance analytics retrieved from cache.");
            }
        }

        var quizPassRates = await _mediator.Send(new GetQuizPassRatesQuery(request.DiplomaId, request.StartDate, request.EndDate), cancellationToken);
        var diplomaAverageScores = await _mediator.Send(new GetDiplomaAverageScoresQuery(request.DiplomaId, request.StartDate, request.EndDate), cancellationToken);
        var attemptsOverTime = await _mediator.Send(new GetAttemptsOverTimeQuery(request.DiplomaId, request.StartDate, request.EndDate), cancellationToken);
        var topFailedQuestions = await _mediator.Send(new GetTopFailedQuestionsQuery(request.DiplomaId, request.StartDate, request.EndDate), cancellationToken);

        var viewModel = new PerformanceAnalyticsResponseViewModel(
            QuizPassRates: quizPassRates,
            DiplomaAverageScores: diplomaAverageScores,
            AttemptsOverTime: attemptsOverTime,
            TopFailedQuestions: topFailedQuestions,
            GeneratedAtUtc: DateTime.UtcNow
        );

        _memoryCache.Set(cacheKey, viewModel, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return RequestResponse<PerformanceAnalyticsResponseViewModel>.Ok(viewModel, "Performance analytics retrieved successfully.");
    }
}
