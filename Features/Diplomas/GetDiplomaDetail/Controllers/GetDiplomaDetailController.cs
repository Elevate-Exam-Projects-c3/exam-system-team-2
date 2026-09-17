using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Orchestrators;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Controllers
{
    [Tags(tags: "Diplomas")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetDiplomaDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetDiplomaDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{diplomaId}")]
        public async Task<IActionResult> GetDiplomaDetail(Guid diplomaId)
        {
            //var studentId = "aaaaaaaa-1111-1111-1111-aaaaaaaaaaaa";
            var requestResponse = await _mediator.Send(new GetDiplomaDetailOrchestrator(diplomaId));
            var endpointResponse = new EndpointResponse
            {
                Success = requestResponse.Success,
                Message = requestResponse.Message,
                Data = requestResponse.Data
            };
            if (!endpointResponse.Success)
                return BadRequest(endpointResponse);

            return Ok(endpointResponse);
        }
    }
}
