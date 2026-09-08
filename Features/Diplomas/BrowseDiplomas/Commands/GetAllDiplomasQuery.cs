using exam_system.Features.Diplomas.AdminCreateDiploma;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Commands
{
    public record GetAllDiplomasQuery(int PageNumber, int PageSize):IRequest<RequestResponse<PaginatedResult<DiplomaDto>>>
    {
    }
}
