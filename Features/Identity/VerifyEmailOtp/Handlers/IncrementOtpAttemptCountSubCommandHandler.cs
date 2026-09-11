using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class IncrementOtpAttemptCountSubCommandHandler : IRequestHandler<IncrementOtpAttemptCountSubCommand, bool>
{
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;

    public IncrementOtpAttemptCountSubCommandHandler(IGenericRepository<EmailVerificationOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(IncrementOtpAttemptCountSubCommand request, CancellationToken cancellationToken)
    {
        request.Otp.AttemptCount += 1;
        _otpRepository.Update(request.Otp);
        return Task.FromResult(true);
    }
}
