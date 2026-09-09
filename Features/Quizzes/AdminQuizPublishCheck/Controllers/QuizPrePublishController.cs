using exam_system.Features.Quizzes.AdminQuizPublishCheck.Mappings;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Controllers
{

    [ApiController]
    [Route("api/admin/quizzes")]
    public class QuizPrePublishController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizPrePublishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<QuizPublishReadinessViewModel>>> GetQuizPublishReadiness(Guid id)
        {
            
            var result = await _mediator.Send(new GetQuizPublishReadinessQuery(id));

            var viewModel = result.Data?.ToViewModel();

            var response = new EndpointResponse<QuizPublishReadinessViewModel>(

                result.Success,
                result.StatusCode,
                result.Message,
                viewModel,
                result.Errors
               
                );

            return StatusCode(result.StatusCode, response);
        }
    }
}
