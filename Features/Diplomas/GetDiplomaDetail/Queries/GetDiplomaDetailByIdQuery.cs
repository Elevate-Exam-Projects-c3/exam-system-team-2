using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Queries
{
    public record GetDiplomaDetailByIdQuery(Guid DiplomaId) : IRequest<RequestResponse<DiplomaDetailDto?>>;
}
