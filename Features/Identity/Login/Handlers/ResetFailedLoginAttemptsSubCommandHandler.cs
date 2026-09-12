using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

public class ResetFailedLoginAttemptsSubCommandHandler : IRequestHandler<ResetFailedLoginAttemptsSubCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public ResetFailedLoginAttemptsSubCommandHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> Handle(ResetFailedLoginAttemptsSubCommand request, CancellationToken cancellationToken)
    {
        request.User.FailedLoginAttempts = 0;
        request.User.LockoutEnd = null;

        _userRepository.Update(request.User);
        return Task.FromResult(true);
    }
}
