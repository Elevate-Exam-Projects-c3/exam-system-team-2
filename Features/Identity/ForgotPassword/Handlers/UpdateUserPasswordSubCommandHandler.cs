using MediatR;
using exam_system.Common.Services.Interfaces;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class UpdateUserPasswordSubCommandHandler : IRequestHandler<UpdateUserPasswordSubCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserPasswordSubCommandHandler(
        IGenericRepository<ApplicationUser> userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public Task<bool> Handle(UpdateUserPasswordSubCommand request, CancellationToken cancellationToken)
    {
        request.User.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        request.User.FailedLoginAttempts = 0;
        request.User.LockoutEnd = null;

        _userRepository.Update(request.User);
        return Task.FromResult(true);
    }
}
