using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class GetLatestActiveOtpByEmailQueryHandler : IRequestHandler<GetLatestActiveOtpByEmailQuery, EmailVerificationOtp?>
{
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;

    public GetLatestActiveOtpByEmailQueryHandler(IGenericRepository<EmailVerificationOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public async Task<EmailVerificationOtp?> Handle(GetLatestActiveOtpByEmailQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        return await _otpRepository
            .Get(o => o.Email == normalizedEmail && !o.IsUsed)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
