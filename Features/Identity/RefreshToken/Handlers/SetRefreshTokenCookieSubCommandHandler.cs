using exam_system.Features.Identity.RefreshToken.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class SetRefreshTokenCookieSubCommandHandler : IRequestHandler<SetRefreshTokenCookieSubCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SetRefreshTokenCookieSubCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<bool> Handle(SetRefreshTokenCookieSubCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = request.ExpiresAt
            };

            httpContext.Response.Cookies.Append("refreshToken", request.RefreshToken, cookieOptions);
        }

        return Task.FromResult(true);
    }
}
