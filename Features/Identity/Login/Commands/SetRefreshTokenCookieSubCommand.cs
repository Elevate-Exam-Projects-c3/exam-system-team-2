using MediatR;

namespace exam_system.Features.Identity.Login.Commands;

public record SetRefreshTokenCookieSubCommand(string RefreshToken, DateTime ExpiresAt) : IRequest<bool>;
