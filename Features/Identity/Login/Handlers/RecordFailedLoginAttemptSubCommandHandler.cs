using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Login.Handlers;

public class RecordFailedLoginAttemptSubCommandHandler : IRequestHandler<RecordFailedLoginAttemptSubCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public RecordFailedLoginAttemptSubCommandHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> Handle(RecordFailedLoginAttemptSubCommand request, CancellationToken cancellationToken)
    {
        request.User.FailedLoginAttempts += 1;
        if (request.User.FailedLoginAttempts >= 5)
        {
            request.User.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
        }

        _userRepository.Update(request.User);
        return Task.FromResult(true);
    }
}
