using MediatR;

namespace exam_system.Features.Identity.Register.Queries;

public record CheckEmailExistsQuery(string Email) : IRequest<bool>;
