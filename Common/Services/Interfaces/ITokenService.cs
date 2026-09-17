using exam_system.Domain.Entities.Identity;

namespace exam_system.Common.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    (string Token, DateTime ExpiresAt) GenerateRefreshToken();
}
