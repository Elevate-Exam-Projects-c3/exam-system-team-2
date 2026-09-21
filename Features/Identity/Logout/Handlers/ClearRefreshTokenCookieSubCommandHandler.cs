using exam_system.Features.Identity.Logout.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace exam_system.Features.Identity.Logout.Handlers;

public class ClearRefreshTokenCookieSubCommandHandler : IRequestHandler<ClearRefreshTokenCookieSubCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClearRefreshTokenCookieSubCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<bool> Handle(ClearRefreshTokenCookieSubCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            httpContext.Response.Cookies.Delete("refreshToken", cookieOptions);
        }

        return Task.FromResult(true);
    }
}
