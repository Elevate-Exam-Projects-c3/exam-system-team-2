using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiplomaController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public DiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region CRUD Operations

        [HttpPost]
        public async Task<IActionResult> CreateDiploma([FromBody] DiplomaViewModel  diplomaViewModel)
        {
            //var command = new CreateDiplomaCommand(diplomaViewModel);
            //var result = await _mediator.Send(command);
            return Ok();
        }
        #endregion

    }
}
