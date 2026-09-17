using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class ActivateUserAccountSubCommandHandler : IRequestHandler<ActivateUserAccountSubCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public ActivateUserAccountSubCommandHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> Handle(ActivateUserAccountSubCommand request, CancellationToken cancellationToken)
    {
        request.User.AccountStatus = AccountStatus.Active;
        request.User.EmailConfirmed = true;
        _userRepository.Update(request.User);
        return Task.FromResult(true);
    }
}
