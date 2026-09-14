using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class GetLatestPasswordResetOtpQueryHandler : IRequestHandler<GetLatestPasswordResetOtpQuery, PasswordResetOtp?>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepository;

    public GetLatestPasswordResetOtpQueryHandler(IGenericRepository<PasswordResetOtp> otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public async Task<PasswordResetOtp?> Handle(GetLatestPasswordResetOtpQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        return await _otpRepository.GetAll()
            .Where(o => o.Email == normalizedEmail)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
