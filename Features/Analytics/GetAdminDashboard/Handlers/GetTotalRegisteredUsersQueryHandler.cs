using exam_system.Domain.Entities.Identity;
using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Handlers;

public class GetTotalRegisteredUsersQueryHandler : IRequestHandler<GetTotalRegisteredUsersQuery, int>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetTotalRegisteredUsersQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(GetTotalRegisteredUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.CountAsync(u => !u.IsDeleted);
    }
}
