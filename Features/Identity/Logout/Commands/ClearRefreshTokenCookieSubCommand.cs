using MediatR;

namespace exam_system.Features.Identity.Logout.Commands;

public record ClearRefreshTokenCookieSubCommand() : IRequest<bool>;
