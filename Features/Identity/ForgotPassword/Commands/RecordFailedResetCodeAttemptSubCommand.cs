using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record RecordFailedResetCodeAttemptSubCommand(PasswordResetOtp Otp) : IRequest<bool>;
