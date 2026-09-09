using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Controllers;

[ApiController]
[Route("api/auth")]
public class RegisterController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<EndpointResponse<RegisterResponse>>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        var response = EndpointResponse<RegisterResponse>.FromResult(result);

        return StatusCode(result.StatusCode, response);
    }
}
