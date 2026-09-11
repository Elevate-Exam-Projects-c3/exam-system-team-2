using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record ConsumeOtpSubCommand(EmailVerificationOtp Otp) : IRequest<bool>;
