using System.Security.Cryptography;
using System.Text;
using exam_system.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace exam_system.Common.Services;

public sealed class OtpService : IOtpService
{
    private readonly byte[] _key;

    public OtpService(IConfiguration configuration)
    {
        var secret = configuration["Security:OtpSecretKey"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Configuration 'Security:OtpSecretKey' is required and cannot be empty.");
        }

        _key = Encoding.UTF8.GetBytes(secret);
    }

    public string GenerateNumericOtp(int length = 6)
    {
        if (length < 1 || length > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "OTP length must be between 1 and 9.");
        }

        var max = (int)Math.Pow(10, length);
        var value = RandomNumberGenerator.GetInt32(0, max);
        return value.ToString($"D{length}");
    }

    public string HashOtp(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
        {
            throw new ArgumentException("OTP cannot be null or empty.", nameof(otp));
        }

        using var hmac = new HMACSHA256(_key);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));
        return Convert.ToHexString(hashBytes);
    }

    public bool VerifyOtp(string plainOtp, string hashedOtp)
    {
        if (string.IsNullOrWhiteSpace(plainOtp) || string.IsNullOrWhiteSpace(hashedOtp))
        {
            return false;
        }

        var computedHash = HashOtp(plainOtp);
        var computedBytes = Encoding.UTF8.GetBytes(computedHash);
        var expectedBytes = Encoding.UTF8.GetBytes(hashedOtp);

        return CryptographicOperations.FixedTimeEquals(computedBytes, expectedBytes);
    }
}
