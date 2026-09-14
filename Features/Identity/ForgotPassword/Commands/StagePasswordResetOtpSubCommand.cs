using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record StagePasswordResetOtpSubCommand(Guid UserId, string Email) : IRequest<string>;
