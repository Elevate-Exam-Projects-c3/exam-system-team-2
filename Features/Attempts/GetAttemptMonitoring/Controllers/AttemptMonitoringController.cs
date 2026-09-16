using exam_system.Features.Attempts.GetAttemptMonitoring.Mappings;
using exam_system.Features.Attempts.GetAttemptMonitoring.Queries;
using exam_system.Features.Attempts.GetAttemptMonitoring.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Controllers
{
    [Tags("AttemptMonitoring")]
    [ApiController]
    [Route("api/admin/attempts")]
    [Authorize(Roles = "Admin")]
    public class AttemptMonitoringController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptMonitoringController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{attemptId:guid}")]
        public async Task<ActionResult<EndpointResponse<AttemptDetailForAdminViewModel>>> GetAttemptDetails(Guid attemptId)
        {
            var result = await _mediator.Send(new GetAttemptDetailForAdminQuery(attemptId));

            var viewModelResult = result.MapTo(dto => dto.ToViewModel());

            return StatusCode(viewModelResult.StatusCode , EndpointResponse<AttemptDetailForAdminViewModel>.FromResult(viewModelResult));
        }

    }
}
