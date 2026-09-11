using exam_system.Features.Identity.RefreshToken.Orchestrators;
using exam_system.Features.Identity.RefreshToken.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace exam_system.Features.Identity.RefreshToken.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth-rate-limit")]
public class RefreshTokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public RefreshTokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<EndpointResponse<RefreshTokenResponseViewModel>>> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            var errorResponse = EndpointResponse<RefreshTokenResponseViewModel>.Fail(
                "Refresh token is missing.",
                StatusCodes.Status401Unauthorized);

            return StatusCode(StatusCodes.Status401Unauthorized, errorResponse);
        }

        var request = new RefreshTokenOrchestratorRequest(refreshToken);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<RefreshTokenResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new RefreshTokenResponseViewModel(
            result.Data!.AccessToken,
            result.Data.TokenType,
            result.Data.ExpiresIn,
            result.Message);
        var successResponse = new EndpointResponse<RefreshTokenResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
