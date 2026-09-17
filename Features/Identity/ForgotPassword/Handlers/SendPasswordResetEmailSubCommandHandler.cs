using MediatR;
using Microsoft.Extensions.Logging;
using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.ForgotPassword.Commands;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class SendPasswordResetEmailSubCommandHandler : IRequestHandler<SendPasswordResetEmailSubCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendPasswordResetEmailSubCommandHandler> _logger;

    public SendPasswordResetEmailSubCommandHandler(
        IEmailService emailService,
        ILogger<SendPasswordResetEmailSubCommandHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> Handle(SendPasswordResetEmailSubCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailBody = $"Hello {request.FullName},\n\nYour password reset verification code is: {request.PlainOtp}\n\nThis code will expire in 10 minutes. If you did not request a password reset, please ignore this email.";
            await _emailService.SendEmailAsync(request.Email, "Reset Your Password", emailBody, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", request.Email);
            return false;
        }
    }
}
