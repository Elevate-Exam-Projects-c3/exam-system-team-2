using FluentValidation;
using exam_system.Features.Identity.Login.ViewModels;

namespace exam_system.Features.Identity.Login.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
