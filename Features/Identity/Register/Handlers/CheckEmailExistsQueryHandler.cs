using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Identity.Register.Handlers;

public class CheckEmailExistsQueryHandler : IRequestHandler<CheckEmailExistsQuery, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public CheckEmailExistsQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        return await _userRepository
            .Get(u => u.Email == normalizedEmail)
            .AnyAsync(cancellationToken);
    }
}
