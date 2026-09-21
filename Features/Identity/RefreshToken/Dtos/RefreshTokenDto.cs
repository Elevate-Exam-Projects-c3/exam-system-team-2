namespace exam_system.Features.Identity.RefreshToken.Dtos;

public record RefreshTokenDto(
    string AccessToken,
    string TokenType = "Bearer",
    int ExpiresIn = 900
);
