using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class ConsumePasswordResetOtpSubCommandHandler : IRequestHandler<ConsumePasswordResetOtpSubCommand, bool>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepository;

    public ConsumePasswordResetOtpSubCommandHandler(IGenericRepository<PasswordResetOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(ConsumePasswordResetOtpSubCommand request, CancellationToken cancellationToken)
    {
        request.Otp.IsUsed = true;
        request.Otp.ResetToken = null;

        _otpRepository.Update(request.Otp);
        return Task.FromResult(true);
    }
}
