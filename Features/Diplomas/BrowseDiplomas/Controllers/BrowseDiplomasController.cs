using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers
{
    [Tags(tags: "Diplomas")]
    //[Authorize(Roles = "Student,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BrowseDiplomasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrowseDiplomasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiplomas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(new GetAllDiplomasQuery(pageNumber, pageSize),cancellationToken);

            var endpointResponse = new EndpointResponse<PaginatedResult<DiplomaDto>>
            {
                Success = requestResponse.Success,
                Message = requestResponse.Message,
                Data= requestResponse.Data
            };
            if (!endpointResponse.Success)
                return BadRequest(endpointResponse);

            return Ok(endpointResponse);
        }
    }
}
