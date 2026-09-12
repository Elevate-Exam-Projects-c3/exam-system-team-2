using exam_system.Domain.Entities.Identity;
using MediatR;

namespace exam_system.Features.Identity.Login.Queries;

public record GenerateAccessTokenQuery(ApplicationUser User) : IRequest<string>;
