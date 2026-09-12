using MediatR;

namespace exam_system.Features.Identity.Login.Commands;

public record CreateRefreshTokenSubCommand(Guid UserId, string Token, DateTime ExpiresAt) : IRequest<bool>;
