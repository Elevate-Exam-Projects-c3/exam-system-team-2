using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public record ResetPasswordOrchestratorRequest(
    string Email,
    string ResetToken,
    string NewPassword) : IRequest<RequestResponse<bool>>;
