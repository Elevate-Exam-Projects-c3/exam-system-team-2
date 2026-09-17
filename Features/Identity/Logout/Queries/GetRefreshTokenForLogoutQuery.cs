using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.Logout.Queries;

public record GetRefreshTokenForLogoutQuery(string RefreshToken) : IRequest<Domain.Entities.Identity.RefreshToken?>;
