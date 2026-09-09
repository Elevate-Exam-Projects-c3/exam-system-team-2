using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

public record RegisterCommand(string FullName, string Email, string Password) : IRequest<RequestResponse<RegisterResponse>>;


