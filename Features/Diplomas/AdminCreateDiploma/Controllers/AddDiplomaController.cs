using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Diplomas")]
    [ApiController]
    public class AddDiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateDiploma([FromBody] DiplomaViewModel diplomaViewModel, CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(
                new AddDiplomaCommand(
                    diplomaViewModel.Title,
                    diplomaViewModel.Description,
                    diplomaViewModel.ImageUrl), cancellationToken);

            var endpointResponse = EndpointResponse<Unit>.FromResult(requestResponse);

            return StatusCode(endpointResponse.StatusCode, endpointResponse);
        }

    }
}
