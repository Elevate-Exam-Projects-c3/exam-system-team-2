using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Controllers;

[ApiController]
[Route("api/auth")]
public class VerifyEmailOtpController : ControllerBase
{
    private readonly IMediator _mediator;

    public VerifyEmailOtpController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("verify-otp")]
    public async Task<ActionResult<EndpointResponse<VerifyEmailOtpResponse>>> VerifyOtp(
        [FromBody] VerifyEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        var response = EndpointResponse<VerifyEmailOtpResponse>.FromResult(result);

        return StatusCode(result.StatusCode, response);
    }
}
