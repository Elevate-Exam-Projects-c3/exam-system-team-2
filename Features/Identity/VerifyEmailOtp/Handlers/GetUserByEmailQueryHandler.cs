using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApplicationUser?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetUserByEmailQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        return await _userRepository
            .Get(u => u.Email == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
