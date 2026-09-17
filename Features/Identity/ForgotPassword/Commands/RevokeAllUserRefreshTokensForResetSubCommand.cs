using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record RevokeAllUserRefreshTokensForResetSubCommand(Guid UserId) : IRequest<bool>;
