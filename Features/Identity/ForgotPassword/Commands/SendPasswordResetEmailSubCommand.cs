using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record SendPasswordResetEmailSubCommand(string Email, string FullName, string PlainOtp) : IRequest<bool>;
