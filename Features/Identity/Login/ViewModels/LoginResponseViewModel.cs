namespace exam_system.Features.Identity.Login.ViewModels;

public record LoginResponseViewModel(
    string AccessToken,
    string TokenType = "Bearer",
    int ExpiresIn = 900,
    string Message = "Login successful."
);
