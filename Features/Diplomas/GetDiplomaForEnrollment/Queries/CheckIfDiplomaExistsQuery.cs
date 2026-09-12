using exam_system.Features.Diplomas.GetDiplomaForEnrollment.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaForEnrollment.Queries
{
    public record CheckIfDiplomaExistsQueryQuery(Guid DiplomaId) : IRequest<RequestResponse<Unit>>;
}
