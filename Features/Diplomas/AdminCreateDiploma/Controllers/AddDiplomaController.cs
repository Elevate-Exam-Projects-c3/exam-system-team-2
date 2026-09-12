using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDiplomaController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public AddDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region CRUD Operations


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDiploma([FromBody] DiplomaViewModel  diplomaViewModel)
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
            var requestResponse = await _mediator.Send(
                new AddDiplomaCommand(
                    diplomaViewModel.Title, 
                    diplomaViewModel.Description, 
                    diplomaViewModel.ImageUrl));

            var endpointResponse = new EndpointResponse
            {
                Success = requestResponse.Success,
                Message = requestResponse.Message,
                Data = requestResponse.Data
            };
            if(!endpointResponse.Success)
                return BadRequest(endpointResponse);

            return Ok(endpointResponse);
        }
        #endregion

    }
}
