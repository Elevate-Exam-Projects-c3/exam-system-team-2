using FluentValidation;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;

namespace exam_system.Features.Identity.ForgotPassword.Validators;

public class ForgotPasswordOrchestratorRequestValidator : AbstractValidator<ForgotPasswordOrchestratorRequest>
{
    public ForgotPasswordOrchestratorRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");
    }
}
