using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Shared;
using MediatR;
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

        [HttpPost]
        public async Task<IActionResult> CreateDiploma([FromBody] DiplomaViewModel  diplomaViewModel)
        {
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
