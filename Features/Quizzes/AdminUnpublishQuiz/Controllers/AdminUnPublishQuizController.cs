using exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Controllers
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class AdminUnPublishQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminUnPublishQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{id:guid}/unpublish")]
        public async Task<ActionResult<EndpointResponse>> UnpublishQuiz(Guid id)
        {
            var result = await _mediator.Send(new UnpublishQuizOrchestrator(id));
            return StatusCode(result.StatusCode, EndpointResponse.FromResult(result));
        }
    }
}
