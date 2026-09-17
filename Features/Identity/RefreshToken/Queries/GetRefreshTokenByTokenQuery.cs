using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Queries;

public record GetRefreshTokenByTokenQuery(string Token) : IRequest<Domain.Entities.Identity.RefreshToken?>;
