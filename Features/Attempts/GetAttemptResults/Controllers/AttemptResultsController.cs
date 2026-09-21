using exam_system.Features.Attempts.GetAttemptResults.Mappings;
using exam_system.Features.Attempts.GetAttemptResults.Queries;
using exam_system.Features.Attempts.GetAttemptResults.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exam_system.Features.Attempts.GetAttemptResults.Controllers
{

    [ApiController]
    [Route("api/attempts")]
    public class AttemptResultsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptResultsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize]
        [HttpGet("{attemptId:guid}/results")]
        public async Task<ActionResult<EndpointResponse<AttemptResultsViewModel>>> GetResults(Guid attemptId)
        {
            var callerUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(new GetAttemptResultsQuery(attemptId, callerUserId));

            var viewModelResult = result.MapTo(dto => dto.ToViewModel());

            return StatusCode(viewModelResult.StatusCode, EndpointResponse<AttemptResultsViewModel>.FromResult(viewModelResult));
        }
    }
}
