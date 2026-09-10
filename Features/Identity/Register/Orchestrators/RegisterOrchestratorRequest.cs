using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.Register.Orchestrators;

public record RegisterOrchestratorRequest(string FullName, string Email, string Password) 
    : IRequest<RequestResponse<RegisterResponse>>;
