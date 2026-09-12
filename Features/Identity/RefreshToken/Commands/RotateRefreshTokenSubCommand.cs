using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Commands;

public record RotateRefreshTokenSubCommand(Domain.Entities.Identity.RefreshToken OldToken, string NewToken, DateTime ExpiresAt) : IRequest<bool>;
