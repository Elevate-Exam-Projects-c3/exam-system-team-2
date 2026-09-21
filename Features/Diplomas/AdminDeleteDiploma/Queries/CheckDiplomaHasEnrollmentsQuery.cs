using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Queries
{
    public record CheckDiplomaHasEnrollmentsQuery(Guid DiplomaId) : IRequest<RequestResponse<Unit>>;

}
