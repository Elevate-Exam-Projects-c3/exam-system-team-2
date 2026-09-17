using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

public record CreateUserSubCommand(string FullName, string Email, string Password) : IRequest<ApplicationUser>;
