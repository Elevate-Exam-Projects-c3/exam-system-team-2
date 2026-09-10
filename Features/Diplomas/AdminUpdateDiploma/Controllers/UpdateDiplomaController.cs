using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.ViewModels;
using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateDiplomaController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public UpdateDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region CRUD Operations
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody]UpdateDiplomaViewModel diploma)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestResponse = await _mediator.Send(new UpdateDiplomaCommand(id, diploma.Title, diploma.Description, diploma.ImageUrl));

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
        #endregion
    }
}
