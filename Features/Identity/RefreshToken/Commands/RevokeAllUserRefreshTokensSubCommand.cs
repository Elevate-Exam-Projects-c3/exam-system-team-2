using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Commands;

public record RevokeAllUserRefreshTokensSubCommand(Guid UserId) : IRequest<bool>;
