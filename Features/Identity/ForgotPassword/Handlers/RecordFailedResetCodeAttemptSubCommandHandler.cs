using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class RecordFailedResetCodeAttemptSubCommandHandler : IRequestHandler<RecordFailedResetCodeAttemptSubCommand, bool>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepository;

    public RecordFailedResetCodeAttemptSubCommandHandler(IGenericRepository<PasswordResetOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(RecordFailedResetCodeAttemptSubCommand request, CancellationToken cancellationToken)
    {
        request.Otp.AttemptCount += 1;
        if (request.Otp.AttemptCount >= 5)
        {
            request.Otp.IsUsed = true;
        }

        _otpRepository.Update(request.Otp);
        return Task.FromResult(true);
    }
}
