using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Register.Orchestrators;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResponse<RegisterResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        
        var exists = await _mediator.Send(new CheckEmailExistsQuery(request.Email), cancellationToken);
        if (exists)
        {
            return RequestResponse<RegisterResponse>.Fail("Email already registered.", 409);
        }

       
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        CreateEmailVerificationOtpResult otpResult;
        ApplicationUser user;

        try
        {
            // Stage user in EF Core Change Tracker (in-memory)
            user = await _mediator.Send(
                new CreateUserSubCommand(request.FullName, request.Email, request.Password),
                cancellationToken);

            // Stage OTP in EF Core Change Tracker (in-memory)
            otpResult = await _mediator.Send(
                new CreateEmailVerificationOtpSubCommand(user.Id, user.Email),
                cancellationToken);

            // ─── 3. Single Atomic Database Commit 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        
        await _mediator.Send(
            new SendEmailVerificationOtpSubCommand(user.Email, user.FullName, otpResult.PlainOtp),
            cancellationToken);


        return RequestResponse<RegisterResponse>.Created(
            new RegisterResponse(user.Id, user.Email, "User registered successfully. Please verify your email with the OTP sent."));
    }
}
