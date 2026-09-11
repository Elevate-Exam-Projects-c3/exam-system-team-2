using exam_system.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace exam_system.Common.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending email to {Email} | Subject: {Subject} | Body: {Body}", toEmail, subject, body);
        return Task.CompletedTask;
    }
}
