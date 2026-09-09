using exam_system.Common.Enums;
using exam_system.Common.Services.Interfaces;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Identity.Register.Handlers;

public class CreateUserSubCommandHandler : IRequestHandler<CreateUserSubCommand, ApplicationUser>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserSubCommandHandler(
        IGenericRepository<ApplicationUser> userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApplicationUser> Handle(CreateUserSubCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            Role = UserRole.Student,
            AccountStatus = AccountStatus.Pending,
            EmailConfirmed = false,
            Student = new Student
            {
                Id = Guid.NewGuid()
            }
        };

        // Stage in EF Core Change Tracker (in-memory) — do NOT call SaveChanges
        await _userRepository.AddAsync(user);

        return user;
    }
}
