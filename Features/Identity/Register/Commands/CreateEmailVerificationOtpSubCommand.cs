using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

public record CreateEmailVerificationOtpSubCommand(Guid UserId, string Email) : IRequest<CreateEmailVerificationOtpResult>;
