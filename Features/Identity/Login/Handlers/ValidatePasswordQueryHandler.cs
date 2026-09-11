using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.Login.Queries;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

public class ValidatePasswordQueryHandler : IRequestHandler<ValidatePasswordQuery, bool>
{
    private readonly IPasswordHasher _passwordHasher;

    public ValidatePasswordQueryHandler(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public Task<bool> Handle(ValidatePasswordQuery request, CancellationToken cancellationToken)
    {
        var isValid = _passwordHasher.VerifyPassword(request.Password, request.PasswordHash);
        return Task.FromResult(isValid);
    }
}
