using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Orchestrators
{
    public record GetDiplomaDetailOrchestrator(Guid DiplomaId) : IRequest<RequestResponse<ViewDiplomaDetailsDto>>;
}
