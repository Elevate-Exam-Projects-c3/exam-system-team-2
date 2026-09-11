using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Orchestrators;
using exam_system.Features.Identity.Register.ViewModels;
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
    public async Task<ActionResult<EndpointResponse<RegisterResponseViewModel>>> Register(
        [FromBody] RegisterViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var request = new RegisterOrchestratorRequest(viewModel.FullName, viewModel.Email, viewModel.Password);
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.Success)
        {
            var errorResponse = new EndpointResponse<RegisterResponseViewModel>(
                false,
                result.StatusCode,
                result.Message,
                null,
                result.Errors);

            return StatusCode(result.StatusCode, errorResponse);
        }

        var responseVm = new RegisterResponseViewModel(result.Data, result.Message);
        var successResponse = new EndpointResponse<RegisterResponseViewModel>(
            true,
            result.StatusCode,
            result.Message,
            responseVm);

        return StatusCode(result.StatusCode, successResponse);
    }
}
