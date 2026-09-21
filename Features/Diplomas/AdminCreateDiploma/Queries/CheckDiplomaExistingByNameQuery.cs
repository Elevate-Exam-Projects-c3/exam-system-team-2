using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Queries
{
    public record CheckDiplomaExistingByNameQuery(string Title) : IRequest<RequestResponse<Unit>>;
}
