using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.Logout.Orchestrators;

public record LogoutOrchestratorRequest(string? RefreshToken) : IRequest<RequestResponse<bool>>;
