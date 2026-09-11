using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record ActivateUserAccountSubCommand(ApplicationUser User) : IRequest<bool>;
