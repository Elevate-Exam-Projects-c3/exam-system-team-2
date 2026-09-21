namespace exam_system.Features.Identity.ForgotPassword.ViewModels;

public record ResetPasswordViewModel(
    string Email,
    string ResetToken,
    string NewPassword,
    string ConfirmPassword);
