using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.Login.Commands;

public record RecordFailedLoginAttemptSubCommand(ApplicationUser User) : IRequest<bool>;
