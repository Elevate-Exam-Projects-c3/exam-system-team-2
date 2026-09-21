using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exam_system.Features.Diplomas.EnrollDiploma.Controllers
{
    [Tags(tags: "Diplomas")]
    //[Authorize(Roles = "Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollDiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EnrollDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Roles = "Student")]
        [HttpPost("{diplomaId}")]
        public async Task<IActionResult> EnrollDiploma([FromRoute] Guid diplomaId,CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(new EnrollDiplomaOrchestrator(diplomaId),cancellationToken);

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
