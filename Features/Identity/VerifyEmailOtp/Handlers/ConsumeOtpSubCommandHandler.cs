using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class ConsumeOtpSubCommandHandler : IRequestHandler<ConsumeOtpSubCommand, bool>
{
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;

    public ConsumeOtpSubCommandHandler(IGenericRepository<EmailVerificationOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(ConsumeOtpSubCommand request, CancellationToken cancellationToken)
    {
        request.Otp.IsUsed = true;
        _otpRepository.Update(request.Otp);
        return Task.FromResult(true);
    }
}
