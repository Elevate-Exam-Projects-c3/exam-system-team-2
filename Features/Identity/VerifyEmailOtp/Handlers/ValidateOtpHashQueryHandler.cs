using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class ValidateOtpHashQueryHandler : IRequestHandler<ValidateOtpHashQuery, bool>
{
    private readonly IOtpService _otpService;

    public ValidateOtpHashQueryHandler(IOtpService otpService)
    {
        _otpService = otpService;
    }

    public Task<bool> Handle(ValidateOtpHashQuery request, CancellationToken cancellationToken)
    {
        var isValid = _otpService.VerifyOtp(request.PlainOtp, request.HashedOtp);
        return Task.FromResult(isValid);
    }
}
