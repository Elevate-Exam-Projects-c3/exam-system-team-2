using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Services.Interfaces;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class StagePasswordResetOtpSubCommandHandler : IRequestHandler<StagePasswordResetOtpSubCommand, string>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepository;
    private readonly IOtpService _otpService;

    public StagePasswordResetOtpSubCommandHandler(
        IGenericRepository<PasswordResetOtp> otpRepository,
        IOtpService otpService)
    {
        _otpRepository = otpRepository;
        _otpService = otpService;
    }

    public async Task<string> Handle(StagePasswordResetOtpSubCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        // Invalidate prior unused OTPs for this email
        var priorActiveOtps = await _otpRepository.GetAll()
            .Where(o => o.Email == normalizedEmail && !o.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var priorOtp in priorActiveOtps)
        {
            priorOtp.IsUsed = true;
            _otpRepository.Update(priorOtp);
        }

        var plainOtp = _otpService.GenerateNumericOtp(6);
        var otpHash = _otpService.HashOtp(plainOtp);

        var otp = new PasswordResetOtp
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Email = normalizedEmail,
            OtpHash = otpHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            AttemptCount = 0,
            IsUsed = false
        };

        await _otpRepository.AddAsync(otp);

        return plainOtp;
    }
}
