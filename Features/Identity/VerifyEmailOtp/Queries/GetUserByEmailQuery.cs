using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.VerifyEmailOtp.Queries;

public record GetUserByEmailQuery(string Email) : IRequest<ApplicationUser?>;
