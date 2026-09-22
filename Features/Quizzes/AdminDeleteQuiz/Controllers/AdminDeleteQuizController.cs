using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/quizzes")]
    public class AdminDeleteQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminDeleteQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EndpointResponse<bool>>> DeleteQuiz(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteQuizOrchestrator(id), cancellationToken);
            return StatusCode(result.StatusCode, EndpointResponse<bool>.FromResult(result));
        }
    }
}
