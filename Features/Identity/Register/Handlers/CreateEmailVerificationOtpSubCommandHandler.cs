using exam_system.Common.Services.Interfaces;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Register.Handlers;

public class CreateEmailVerificationOtpSubCommandHandler : IRequestHandler<CreateEmailVerificationOtpSubCommand, CreateEmailVerificationOtpResult>
{
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;
    private readonly IOtpService _otpService;

    public CreateEmailVerificationOtpSubCommandHandler(
        IGenericRepository<EmailVerificationOtp> otpRepository,
        IOtpService otpService)
    {
        _otpRepository = otpRepository;
        _otpService = otpService;
    }

    public async Task<CreateEmailVerificationOtpResult> Handle(CreateEmailVerificationOtpSubCommand request, CancellationToken cancellationToken)
    {
        var plainOtp = _otpService.GenerateNumericOtp(6);
        var otpHash = _otpService.HashOtp(plainOtp);

        var otp = new EmailVerificationOtp
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Email = request.Email.Trim().ToLower(),
            OtpHash = otpHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            AttemptCount = 0,
            IsUsed = false
        };

        // Stage in EF Core Change Tracker (in-memory) — do NOT call SaveChanges
        await _otpRepository.AddAsync(otp);

        return new CreateEmailVerificationOtpResult(otp.Id, plainOtp);
    }
}
