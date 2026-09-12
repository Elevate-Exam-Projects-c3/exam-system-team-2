using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record IncrementOtpAttemptCountSubCommand(EmailVerificationOtp Otp) : IRequest<bool>;
