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
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollDiplomaController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public EnrollDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region CRUD Operations
        [Authorize(Roles = "Student")]
        [HttpPost("{diplomaId}")]
        public async Task<IActionResult> EnrollDiploma([FromRoute] Guid diplomaId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray());

                return BadRequest(new EndpointResponse
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var requestResponse = await _mediator.Send(new EnrollDiplomaOrchestrator(diplomaId));
        
            var endpointResponse = new EndpointResponse
            {
                Success = requestResponse.Success,
                Message = requestResponse.Message,
                Data = requestResponse.Data
            };
            if (!endpointResponse.Success)
                return BadRequest(endpointResponse);

            return Ok(endpointResponse);




            return Ok();
        }
        #endregion
    }
}
