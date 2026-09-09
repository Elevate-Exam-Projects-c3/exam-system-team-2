using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

public record CreateEmailVerificationOtpResult(Guid OtpId, string PlainOtp);

public record CreateEmailVerificationOtpSubCommand(Guid UserId, string Email) : IRequest<CreateEmailVerificationOtpResult>;
