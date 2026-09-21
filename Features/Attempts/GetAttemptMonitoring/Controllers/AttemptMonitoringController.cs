using exam_system.Common.Enums;
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

        //1. Get Attempt Details for Admin :
        [HttpGet("{attemptId:guid}")]
        public async Task<ActionResult<EndpointResponse<AttemptDetailForAdminViewModel>>> GetAttemptDetails(Guid attemptId)
        {
            var result = await _mediator.Send(new GetAttemptDetailForAdminQuery(attemptId));

            var viewModelResult = result.MapTo(dto => dto.ToViewModel());

            return StatusCode(viewModelResult.StatusCode , EndpointResponse<AttemptDetailForAdminViewModel>.FromResult(viewModelResult));
        }

        //2. Get Attempts Monitoring List for Admin : 
        [HttpGet]
        public async Task<IActionResult> GetAllAttempts(
            [FromQuery] Guid? quizId,
            [FromQuery] Guid? studentId,
            [FromQuery] AttemptStatus? status,
            [FromQuery] AttemptSortOrder sortOrder = AttemptSortOrder.Descending,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAttemptsQuery( quizId, studentId,status,sortOrder,pageIndex,pageSize));

            var viewModelResult = result.MapTo(dto => dto.ToViewModel());

            return StatusCode( viewModelResult.StatusCode,EndpointResponse<PaginatedResult<AttemptSummaryViewModel>>.FromResult(viewModelResult));
        }
    }
}
