namespace exam_system.Common.Services;

public static class EmailTemplateBuilder
{
    public static string BuildOtpVerificationEmail(string fullName, string otpCode, int expiryMinutes = 10)
    {
        return $"Welcome {fullName}, your verification code is: {otpCode}. It expires in {expiryMinutes} minutes.";
    }
}
