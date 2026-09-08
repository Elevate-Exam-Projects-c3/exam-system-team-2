using exam_system.Features.Diplomas.AdminCreateDiploma;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Queries
{
    public record GetDiplomaDetailByIdQuery(Guid Id):IRequest<RequestResponse<DiplomaDto?>>
    {
    }
}
