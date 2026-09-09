using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record VerifyEmailOtpCommand(string Email, string Otp) : IRequest<RequestResponse<VerifyEmailOtpResponse>>;
