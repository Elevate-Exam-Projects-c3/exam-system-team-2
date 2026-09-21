using FluentValidation;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

namespace exam_system.Features.Identity.VerifyEmailOtp.Validators;

public class VerifyEmailOtpOrchestratorRequestValidator : AbstractValidator<VerifyEmailOtpOrchestratorRequest>
{
    public VerifyEmailOtpOrchestratorRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be exactly 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("OTP must consist of 6 digits.");
    }
}
