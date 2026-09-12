using exam_system.Features.Identity.Logout.Orchestrators;
using exam_system.Features.Identity.Logout.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Identity.Logout.Controllers;

[ApiController]
[Route("api/auth")]
public class LogoutController : ControllerBase
{
    private readonly IMediator _mediator;

    public LogoutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("logout")]
    public async Task<ActionResult<EndpointResponse<LogoutResponseViewModel>>> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var request = new LogoutOrchestratorRequest(refreshToken);
        var result = await _mediator.Send(request, cancellationToken);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<LogoutResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                new LogoutResponseViewModel(false, result.Message),
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new LogoutResponseViewModel(result.Data, result.Message);
        var successResponse = new EndpointResponse<LogoutResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
