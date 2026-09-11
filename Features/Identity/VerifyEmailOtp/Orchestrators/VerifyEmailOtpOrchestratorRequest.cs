using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

public record VerifyEmailOtpOrchestratorRequest(string Email, string Otp) 
    : IRequest<RequestResponse<bool>>;
