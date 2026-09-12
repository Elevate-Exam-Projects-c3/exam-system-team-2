using exam_system.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace exam_system.Common.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly int _workFactor;

    public PasswordHasher(IConfiguration configuration)
    {
        var configuredWorkFactor = configuration["Security:PasswordWorkFactor"];
        if (int.TryParse(configuredWorkFactor, out var parsed) && parsed > 0)
        {
            _workFactor = parsed;
        }
        else
        {
            _workFactor = 12;
        }
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be null or empty.", nameof(passwordHash));
        }

        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
