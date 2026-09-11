using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;
using exam_system.Features.Identity.VerifyEmailOtp.ViewModels;
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
    public async Task<ActionResult<EndpointResponse<VerifyEmailOtpResponseViewModel>>> VerifyOtp(
        [FromBody] VerifyEmailOtpViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new VerifyEmailOtpOrchestratorRequest(viewModel.Email, viewModel.Otp);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<VerifyEmailOtpResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new VerifyEmailOtpResponseViewModel(result.Data, result.Message);
        var successResponse = new EndpointResponse<VerifyEmailOtpResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
