using MediatR;

namespace exam_system.Features.Identity.Login.Queries;

public record ValidatePasswordQuery(string Password, string PasswordHash) : IRequest<bool>;
