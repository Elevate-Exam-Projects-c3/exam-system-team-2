namespace exam_system.Features.Identity.VerifyEmailOtp.ViewModels;

public class VerifyEmailOtpResponseViewModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public VerifyEmailOtpResponseViewModel() { }

    public VerifyEmailOtpResponseViewModel(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}
