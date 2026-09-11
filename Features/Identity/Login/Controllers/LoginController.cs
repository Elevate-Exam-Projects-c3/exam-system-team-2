using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using exam_system.Features.Identity.Login.Orchestrators;
using exam_system.Features.Identity.Login.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Login.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth-rate-limit")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<EndpointResponse<LoginResponseViewModel>>> Login(
        [FromBody] LoginViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new LoginOrchestratorRequest(viewModel.Email, viewModel.Password);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<LoginResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new LoginResponseViewModel(result.Data!, "Bearer", 900, result.Message);
        var successResponse = new EndpointResponse<LoginResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
