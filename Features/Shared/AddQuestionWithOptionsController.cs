using exam_system.Features.Quizzes.AdminManageQuestions.Mappings;
using exam_system.Features.Quizzes.AdminManageQuestions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Shared
{
    [ApiController]
    [Route("api/admin/questions")]
    public class AddQuestionWithOptionsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AddQuestionWithOptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<AddQuestionWithOptionsResponseViewModel>>> AddQuestionWithOptions([FromBody] AddQuestionWithOptionsViewModel request)
        {
            var result = await _mediator.Send(request.ToOrchestrator());

            //Map From ResponseDto to ResponseViewModel
            var viewModelResult = result.MapTo(d => d.ToViewModel());
            return Ok(EndpointResponse<AddQuestionWithOptionsResponseViewModel>.FromResult(viewModelResult));
        }
    }
}
