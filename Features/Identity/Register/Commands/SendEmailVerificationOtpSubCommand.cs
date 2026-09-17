using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

public record SendEmailVerificationOtpSubCommand(string Email, string FullName, string PlainOtp) : IRequest<bool>;
