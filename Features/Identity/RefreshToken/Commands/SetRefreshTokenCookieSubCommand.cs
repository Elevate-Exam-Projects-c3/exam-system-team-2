using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Commands;

public record SetRefreshTokenCookieSubCommand(string RefreshToken, DateTime ExpiresAt) : IRequest<bool>;
