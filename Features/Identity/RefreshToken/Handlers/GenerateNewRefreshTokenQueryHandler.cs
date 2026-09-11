using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.RefreshToken.Queries;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class GenerateNewRefreshTokenQueryHandler : IRequestHandler<GenerateNewRefreshTokenQuery, GenerateRefreshTokenResult>
{
    private readonly ITokenService _tokenService;

    public GenerateNewRefreshTokenQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<GenerateRefreshTokenResult> Handle(GenerateNewRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var (token, expiresAt) = _tokenService.GenerateRefreshToken();
        return Task.FromResult(new GenerateRefreshTokenResult(token, expiresAt));
    }
}
