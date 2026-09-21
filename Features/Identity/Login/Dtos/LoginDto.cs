namespace exam_system.Features.Identity.Login.Dtos;

public record LoginDto(
    string AccessToken,
    string TokenType = "Bearer",
    int ExpiresIn = 900
);
