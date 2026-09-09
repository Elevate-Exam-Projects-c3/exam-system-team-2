using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record ActivateUserAndConsumeOtpSubCommand(ApplicationUser User, EmailVerificationOtp Otp) : IRequest<bool>;
