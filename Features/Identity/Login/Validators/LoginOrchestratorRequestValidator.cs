using FluentValidation;
using exam_system.Features.Identity.Login.Orchestrators;

namespace exam_system.Features.Identity.Login.Validators;

public class LoginOrchestratorRequestValidator : AbstractValidator<LoginOrchestratorRequest>
{
    public LoginOrchestratorRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
