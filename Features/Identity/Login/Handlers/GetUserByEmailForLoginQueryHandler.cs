using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.Login.Handlers;

public class GetUserByEmailForLoginQueryHandler : IRequestHandler<GetUserByEmailForLoginQuery, ApplicationUser?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetUserByEmailForLoginQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> Handle(GetUserByEmailForLoginQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        return await _userRepository
            .Get(u => u.Email == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
