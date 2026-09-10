namespace exam_system.Features.Identity.Register.Commands;

public record CreateEmailVerificationOtpResult(Guid OtpId, string PlainOtp);
