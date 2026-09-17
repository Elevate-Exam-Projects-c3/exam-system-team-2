namespace exam_system.Features.Identity.Register.ViewModels;

public class RegisterResponseViewModel
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;

    public RegisterResponseViewModel(Guid userId, string message)
    {
        UserId = userId;
        Message = message;
    }
}
