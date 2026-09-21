using MediatR;
using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.ForgotPassword.Queries;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class ValidateResetOtpHashQueryHandler : IRequestHandler<ValidateResetOtpHashQuery, bool>
{
    private readonly IOtpService _otpService;

    public ValidateResetOtpHashQueryHandler(IOtpService otpService)
    {
        _otpService = otpService;
    }

    public Task<bool> Handle(ValidateResetOtpHashQuery request, CancellationToken cancellationToken)
    {
        var isValid = _otpService.VerifyOtp(request.PlainOtp, request.HashedOtp);
        return Task.FromResult(isValid);
    }
}
