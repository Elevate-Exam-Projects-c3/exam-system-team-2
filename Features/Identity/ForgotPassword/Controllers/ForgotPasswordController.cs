using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;
using exam_system.Features.Identity.ForgotPassword.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Controllers;

[Tags("Authentication")]
[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IMediator _mediator;

    public ForgotPasswordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<EndpointResponse<ForgotPasswordResponseViewModel>>> ForgotPassword(
        [FromBody] ForgotPasswordViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new ForgotPasswordOrchestratorRequest(viewModel.Email);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<ForgotPasswordResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new ForgotPasswordResponseViewModel(result.Message);
        var successResponse = new EndpointResponse<ForgotPasswordResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }

    [HttpPost("verify-reset-code")]
    public async Task<ActionResult<EndpointResponse<VerifyResetCodeResponseViewModel>>> VerifyResetCode(
        [FromBody] VerifyResetCodeViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new VerifyResetCodeOrchestratorRequest(viewModel.Email, viewModel.Code);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<VerifyResetCodeResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new VerifyResetCodeResponseViewModel(result.Data!, result.Message);
        var successResponse = new EndpointResponse<VerifyResetCodeResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<EndpointResponse<ResetPasswordResponseViewModel>>> ResetPassword(
        [FromBody] ResetPasswordViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new ResetPasswordOrchestratorRequest(viewModel.Email, viewModel.ResetToken, viewModel.NewPassword);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<ResetPasswordResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new ResetPasswordResponseViewModel(result.Message);
        var successResponse = new EndpointResponse<ResetPasswordResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
