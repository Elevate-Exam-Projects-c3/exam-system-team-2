using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class ActivateUserAndConsumeOtpSubCommandHandler : IRequestHandler<ActivateUserAndConsumeOtpSubCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;

    public ActivateUserAndConsumeOtpSubCommandHandler(
        IGenericRepository<ApplicationUser> userRepository,
        IGenericRepository<EmailVerificationOtp> otpRepository)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(ActivateUserAndConsumeOtpSubCommand request, CancellationToken cancellationToken)
    {
        // 1. Mark OTP as used
        request.Otp.IsUsed = true;
        _otpRepository.Update(request.Otp);

        // 2. Activate user account and confirm email
        request.User.AccountStatus = AccountStatus.Active;
        request.User.EmailConfirmed = true;
        _userRepository.Update(request.User);

        return Task.FromResult(true);
    }
}
