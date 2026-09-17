using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public record ForgotPasswordOrchestratorRequest(string Email) : IRequest<RequestResponse<bool>>;
