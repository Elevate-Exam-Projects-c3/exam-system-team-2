using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries
{
    public record GetAllDiplomasQuery(int PageNumber, int PageSize) : IRequest<RequestResponse<PaginatedResult<DiplomaDto>>>;
}
