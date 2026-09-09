using exam_system.Common.Services;
using exam_system.Common.Services.Interfaces;
using exam_system.Features.Identity.Register.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace exam_system.Features.Identity.Register.Handlers;

public class SendEmailVerificationOtpSubCommandHandler : IRequestHandler<SendEmailVerificationOtpSubCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailVerificationOtpSubCommandHandler> _logger;

    public SendEmailVerificationOtpSubCommandHandler(
        IEmailService emailService,
        ILogger<SendEmailVerificationOtpSubCommandHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> Handle(SendEmailVerificationOtpSubCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailBody = EmailTemplateBuilder.BuildOtpVerificationEmail(request.FullName, request.PlainOtp);
            await _emailService.SendEmailAsync(request.Email, "Verify Your Email", emailBody, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}", request.Email);
            return false;
        }
    }
}
