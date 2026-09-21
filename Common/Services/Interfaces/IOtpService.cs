namespace exam_system.Common.Services.Interfaces;

public interface IOtpService
{
    string GenerateNumericOtp(int length = 6);
    string HashOtp(string otp);
    bool VerifyOtp(string plainOtp, string hashedOtp);
}
