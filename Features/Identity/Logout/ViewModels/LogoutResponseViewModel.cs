namespace exam_system.Features.Identity.Logout.ViewModels;

public record LogoutResponseViewModel(
    bool IsSuccess,
    string Message = "Logged out successfully."
);
