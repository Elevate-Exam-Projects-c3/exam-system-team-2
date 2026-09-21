using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.Logout.Commands;

public record RevokeRefreshTokenSubCommand(Domain.Entities.Identity.RefreshToken Token) : IRequest<bool>;
