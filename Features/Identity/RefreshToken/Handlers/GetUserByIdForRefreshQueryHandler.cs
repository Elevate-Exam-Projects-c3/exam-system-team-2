using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.RefreshToken.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class GetUserByIdForRefreshQueryHandler : IRequestHandler<GetUserByIdForRefreshQuery, ApplicationUser?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetUserByIdForRefreshQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> Handle(GetUserByIdForRefreshQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository
            .Get(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
