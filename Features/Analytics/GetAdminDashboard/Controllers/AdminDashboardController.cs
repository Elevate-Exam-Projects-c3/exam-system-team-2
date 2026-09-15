using exam_system.Features.Analytics.GetAdminDashboard.Orchestrators;
using exam_system.Features.Analytics.GetAdminDashboard.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Analytics.GetAdminDashboard.Controllers;

[Tags("Analytics")]
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/dashboard")]
public class AdminDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("snapshot")]
    public async Task<ActionResult<EndpointResponse<AdminDashboardSnapshotViewModel>>> GetSnapshot(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminDashboardSnapshotOrchestratorRequest(), cancellationToken);
        return StatusCode(result.StatusCode, EndpointResponse<AdminDashboardSnapshotViewModel>.FromResult(result));
    }
}
