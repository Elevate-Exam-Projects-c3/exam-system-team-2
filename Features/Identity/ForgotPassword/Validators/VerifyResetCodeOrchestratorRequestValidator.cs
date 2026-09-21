using FluentValidation;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;

namespace exam_system.Features.Identity.ForgotPassword.Validators;

public class VerifyResetCodeOrchestratorRequestValidator : AbstractValidator<VerifyResetCodeOrchestratorRequest>
{
    public VerifyResetCodeOrchestratorRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Verification code must be exactly 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("Verification code must contain only numbers.");
    }
}
