using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Handlers;

public class GetTotalDiplomasCountQueryHandler : IRequestHandler<GetTotalDiplomasCountQuery, int>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public GetTotalDiplomasCountQueryHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<int> Handle(GetTotalDiplomasCountQuery request, CancellationToken cancellationToken)
    {
        return await _diplomaRepository.CountAsync(d => !d.IsDeleted);
    }
}
