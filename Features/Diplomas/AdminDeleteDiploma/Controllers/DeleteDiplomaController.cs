using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Controllers
{
    [Tags(tags:"Diplomas")]
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteDiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeleteDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiploma([FromRoute] Guid id,CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(
                new DeleteDiplomaCommand(id),cancellationToken);

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
