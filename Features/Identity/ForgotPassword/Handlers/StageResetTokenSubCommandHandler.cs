using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class StageResetTokenSubCommandHandler : IRequestHandler<StageResetTokenSubCommand, bool>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepository;

    public StageResetTokenSubCommandHandler(IGenericRepository<PasswordResetOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public Task<bool> Handle(StageResetTokenSubCommand request, CancellationToken cancellationToken)
    {
        request.Otp.ResetToken = request.ResetToken;
        request.Otp.ResetTokenExpiresAt = request.ExpiresAt;

        _otpRepository.Update(request.Otp);
        return Task.FromResult(true);
    }
}
