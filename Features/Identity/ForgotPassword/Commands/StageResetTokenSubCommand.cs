using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record StageResetTokenSubCommand(
    PasswordResetOtp Otp,
    string ResetToken,
    DateTime ExpiresAt) : IRequest<bool>;
