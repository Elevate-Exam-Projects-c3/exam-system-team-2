using FluentValidation;
using exam_system.Features.Identity.RefreshToken.Orchestrators;

namespace exam_system.Features.Identity.RefreshToken.Validators;

public class RefreshTokenOrchestratorRequestValidator : AbstractValidator<RefreshTokenOrchestratorRequest>
{
    public RefreshTokenOrchestratorRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
