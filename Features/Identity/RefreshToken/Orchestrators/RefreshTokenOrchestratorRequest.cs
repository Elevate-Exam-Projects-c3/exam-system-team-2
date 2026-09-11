using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Orchestrators;

public record RefreshTokenOrchestratorRequest(string RefreshToken) : IRequest<RequestResponse<string>>;
