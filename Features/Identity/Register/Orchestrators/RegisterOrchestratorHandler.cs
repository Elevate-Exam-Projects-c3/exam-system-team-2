using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Register.Orchestrators;

public class RegisterOrchestratorHandler : IRequestHandler<RegisterOrchestratorRequest, RequestResponse<RegisterResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<RegisterResponse>> Handle(RegisterOrchestratorRequest request, CancellationToken cancellationToken)
    {
        // ─── 1. Sub-Query: Validate Email Uniqueness ────────────────────────
        var exists = await _mediator.Send(new CheckEmailExistsQuery(request.Email), cancellationToken);
        if (exists)
        {
            return RequestResponse<RegisterResponse>.Fail("Email already registered.", 409);
        }

        // ─── 2. Transaction Scope for Staging & Commit ──────────────────────
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

            // ─── 3. Single Atomic Database Commit ───────────────────────────
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        // ─── 4. External Side-Effect: Dispatch Email (Outside DB Transaction) ──
        // TODO: Use a background worker or outbox pattern to guarantee reliable email delivery.
        await _mediator.Send(
            new SendEmailVerificationOtpSubCommand(user.Email, user.FullName, otpResult.PlainOtp),
            cancellationToken);

        // ─── 5. Return Safe Response ────────────────────────────────────────
        return RequestResponse<RegisterResponse>.Created(
            new RegisterResponse(user.Id, user.Email, "User registered successfully. Please verify your email with the OTP sent."));
    }
}
