using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Queries;

public record GenerateRefreshTokenResult(string Token, DateTime ExpiresAt);

public record GenerateNewRefreshTokenQuery() : IRequest<GenerateRefreshTokenResult>;
