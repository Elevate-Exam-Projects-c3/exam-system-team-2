using exam_system.Features.Attempts.SubmitQuestionAnswer.Mappings;
using exam_system.Features.Attempts.SubmitQuestionAnswer.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Controllers
{
    [Tags("Attempts")]
    [ApiController]
    [Route("api/attempts")]
    public class AttemptAnswersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptAnswersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Student")]
        [HttpPost("{attemptId:guid}/answers")]
        public async Task<ActionResult<EndpointResponse>> SubmitAnswer(Guid attemptId, [FromBody] SubmitAnswerViewModel request)
        {
            var callerUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orchestratorRequest = request.ToOrchestratorRequest(attemptId, callerUserId);

            var result = await _mediator.Send(orchestratorRequest);

            return StatusCode(result.StatusCode, EndpointResponse.FromResult(result));
        }

    }
}
