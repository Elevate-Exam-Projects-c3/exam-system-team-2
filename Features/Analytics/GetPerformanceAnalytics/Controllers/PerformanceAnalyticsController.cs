using exam_system.Features.Analytics.GetPerformanceAnalytics.Orchestrators;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Controllers;

[Tags("Analytics")]
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/analytics")]
public class PerformanceAnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerformanceAnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("performance")]
    public async Task<ActionResult<EndpointResponse<PerformanceAnalyticsResponseViewModel>>> GetPerformanceAnalytics(
        [FromQuery] PerformanceAnalyticsFilterViewModel filter,
        CancellationToken cancellationToken)
    {
        var request = new GetPerformanceAnalyticsOrchestratorRequest(
            filter.DiplomaId,
            filter.StartDate,
            filter.EndDate
        );

        var result = await _mediator.Send(request, cancellationToken);
        return StatusCode(result.StatusCode, EndpointResponse<PerformanceAnalyticsResponseViewModel>.FromResult(result));
    }
}
