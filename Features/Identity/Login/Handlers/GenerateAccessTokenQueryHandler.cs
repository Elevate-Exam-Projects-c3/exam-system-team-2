using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.Login.Queries;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

public class GenerateAccessTokenQueryHandler : IRequestHandler<GenerateAccessTokenQuery, string>
{
    private readonly ITokenService _tokenService;

    public GenerateAccessTokenQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<string> Handle(GenerateAccessTokenQuery request, CancellationToken cancellationToken)
    {
        var token = _tokenService.GenerateAccessToken(request.User);
        return Task.FromResult(token);
    }
}
