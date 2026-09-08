using exam_system.Features.Diplomas.AdminCreateDiploma;
using exam_system.Features.Diplomas.BrowseDiplomas.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowseDiplomasController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public BrowseDiplomasController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region CRUD Operations
        [HttpGet]
        public async Task<IActionResult> GetAllDiplomas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var requestResponse = await _mediator.Send(new GetAllDiplomasQuery(pageNumber, pageSize));

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
        #endregion
    }
}
