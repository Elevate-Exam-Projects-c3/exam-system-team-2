using MediatR;

namespace exam_system.Features.Identity.Login.Queries;

public record GenerateRefreshTokenResult(string Token, DateTime ExpiresAt);

public record GenerateRefreshTokenQuery() : IRequest<GenerateRefreshTokenResult>;
