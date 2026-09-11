namespace exam_system.Features.Identity.RefreshToken.ViewModels;

public record RefreshTokenResponseViewModel(
    string AccessToken,
    string TokenType = "Bearer",
    int ExpiresIn = 900,
    string Message = "Token refreshed successfully."
);
