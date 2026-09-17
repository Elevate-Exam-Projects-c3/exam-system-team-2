using MediatR;
using exam_system.Common.Services.Interfaces;

namespace exam_system.Features.Identity.VerifyEmailOtp.Queries;

public record ValidateOtpHashQuery(string PlainOtp, string HashedOtp) : IRequest<bool>;
