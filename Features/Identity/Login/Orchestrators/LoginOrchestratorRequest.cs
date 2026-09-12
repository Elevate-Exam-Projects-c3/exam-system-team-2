using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.Login.Orchestrators;

public record LoginOrchestratorRequest(string Email, string Password) : IRequest<RequestResponse<string>>;
