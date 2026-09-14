using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public record VerifyResetCodeOrchestratorRequest(string Email, string Code) : IRequest<RequestResponse<string>>;
