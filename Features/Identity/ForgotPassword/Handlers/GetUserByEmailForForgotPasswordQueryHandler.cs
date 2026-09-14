using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class GetUserByEmailForForgotPasswordQueryHandler : IRequestHandler<GetUserByEmailForForgotPasswordQuery, ApplicationUser?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetUserByEmailForForgotPasswordQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> Handle(GetUserByEmailForForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        return await _userRepository.GetAll()
            .Where(u => u.Email == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
