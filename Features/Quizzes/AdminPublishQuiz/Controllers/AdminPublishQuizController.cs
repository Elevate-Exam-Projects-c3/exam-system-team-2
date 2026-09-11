using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.Mappings;
using exam_system.Features.Quizzes.AdminPublishQuiz.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Controllers
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class AdminPublishQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminPublishQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{quizId:guid}/publish")]
        public async Task<ActionResult<EndpointResponse<PublishQuizResultViewModel>>> Publish(Guid quizId)
        {
            var result = await _mediator.Send(new PublishQuizCommand(quizId));

            var viewModelResult = result.MapTo(r => r.ToViewModel());

            return StatusCode(viewModelResult.StatusCode,EndpointResponse<PublishQuizResultViewModel>.FromResult(viewModelResult));
        }
    }
}
