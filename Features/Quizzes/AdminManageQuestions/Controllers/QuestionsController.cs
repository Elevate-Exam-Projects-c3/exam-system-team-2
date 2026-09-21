using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Mappings;
using exam_system.Features.Quizzes.AdminManageQuestions.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/questions")]
    public class QuestionsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //1. Add Question with Options EndPoint :
        [HttpPost]
        public async Task<ActionResult<EndpointResponse<Guid>>> AddQuestionWithOptions([FromBody] AddQuestionWithOptionsViewModel request ,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request.ToOrchestrator(), cancellationToken);
            return StatusCode(result.StatusCode,EndpointResponse<Guid>.FromResult(result));

        }

        //2. Delete Question EndPoint :

        [HttpDelete("{questionId:Guid}")]
        public async Task<ActionResult<EndpointResponse>> DeleteQuestion(Guid questionId , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteQuestionCommand(questionId), cancellationToken);
            return StatusCode(result.StatusCode, EndpointResponse.FromResult(result));
        }
    }
}
