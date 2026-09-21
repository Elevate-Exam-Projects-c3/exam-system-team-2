using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.RefreshToken.Queries;
using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class GenerateAccessTokenForRefreshQueryHandler : IRequestHandler<GenerateAccessTokenForRefreshQuery, string>
{
    private readonly ITokenService _tokenService;

    public GenerateAccessTokenForRefreshQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<string> Handle(GenerateAccessTokenForRefreshQuery request, CancellationToken cancellationToken)
    {
        var token = _tokenService.GenerateAccessToken(request.User);
        return Task.FromResult(token);
    }
}
