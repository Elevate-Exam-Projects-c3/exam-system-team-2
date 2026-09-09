namespace exam_system.Features.Identity.VerifyEmailOtp;

public record VerifyEmailOtpResponse(Guid UserId, string Email, string Message);
