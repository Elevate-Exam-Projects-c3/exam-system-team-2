using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Queries
{
    public record CheckIfDiplomaExistsQueryQuery(Guid DiplomaId) : IRequest<RequestResponse<Unit>>;
}
