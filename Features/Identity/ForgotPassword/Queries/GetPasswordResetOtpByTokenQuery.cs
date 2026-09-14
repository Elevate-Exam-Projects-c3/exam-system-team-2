using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.ForgotPassword.Queries;

public record GetPasswordResetOtpByTokenQuery(string Email, string ResetToken) : IRequest<PasswordResetOtp?>;
