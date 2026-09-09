using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Common.Services.Interfaces;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using Microsoft.Extensions.Logging;

namespace exam_system.Features.Identity.Register.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResponse<RegisterResponse>>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly IGenericRepository<EmailVerificationOtp> _otpRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;

    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IGenericRepository<ApplicationUser> userRepository,
        IGenericRepository<EmailVerificationOtp> otpRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IOtpService otpService,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _otpService = otpService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RequestResponse<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        var exists = await _userRepository.Get(u => u.Email == normalizedEmail).AnyAsync(cancellationToken);
        if (exists)
        {
            return RequestResponse<RegisterResponse>.Fail("Email already registered.", 409);
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var plainOtp = _otpService.GenerateNumericOtp(6);
        var otpHash = _otpService.HashOtp(plainOtp);

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

        var otp = new EmailVerificationOtp
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Email = normalizedEmail,
            OtpHash = otpHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            AttemptCount = 0,
            IsUsed = false
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userRepository.AddAsync(user);
            await _otpRepository.AddAsync(otp);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to persist user registration for {Email}", normalizedEmail);
            return RequestResponse<RegisterResponse>.Fail("Registration failed due to a system error. Please try again.", 500);
        }

        // Attempt email dispatch outside the database transaction to prevent connection pool starvation.
        // TODO: Use a background worker or outbox pattern to guarantee reliable email delivery.
        try
        {
            var emailBody = EmailTemplateBuilder.BuildOtpVerificationEmail(user.FullName, plainOtp);
            await _emailService.SendEmailAsync(user.Email, "Verify Your Email", emailBody, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}. User can request a resend.", normalizedEmail);
        }

        return RequestResponse<RegisterResponse>.Created(
            new RegisterResponse(user.Id, user.Email, "User registered successfully. Please verify your email with the OTP sent."));
    }
}
