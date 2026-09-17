using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.Login.Queries;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

public class GenerateRefreshTokenQueryHandler : IRequestHandler<GenerateRefreshTokenQuery, GenerateRefreshTokenResult>
{
    private readonly ITokenService _tokenService;

    public GenerateRefreshTokenQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<GenerateRefreshTokenResult> Handle(GenerateRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var (token, expiresAt) = _tokenService.GenerateRefreshToken();
        return Task.FromResult(new GenerateRefreshTokenResult(token, expiresAt));
    }
}
