using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Queries;

public record ValidateResetOtpHashQuery(string PlainOtp, string HashedOtp) : IRequest<bool>;
