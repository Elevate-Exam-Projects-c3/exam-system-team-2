using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using System.Security.Claims;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Persistence.DataAccess;


namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollDiplomaOrchestratorHandler : IRequestHandler<EnrollDiplomaOrchestrator, RequestResponse<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserId _currentUserId;
        private readonly IUnitOfWork _unitOfWork;
        public EnrollDiplomaOrchestratorHandler(IMediator mediator, ICurrentUserId currentUserId, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _currentUserId = currentUserId;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Unit>> Handle(EnrollDiplomaOrchestrator request, CancellationToken cancellationToken)
        {
            //var studentId = _currentUserId.GetStudentId();
            var studentId = Guid.Parse("AAAAAAAA-1111-1111-1111-AAAAAAAAAAAA");
            if (studentId == null)
                return RequestResponse<Unit>.Fail("Student profile not found.", 403);

            RequestResponse<Unit>? result = null;

            await _unitOfWork.ExecuteAsync(async ct =>
            {
                var diplomaExsist = await _mediator.Send(new CheckIfDiplomaExistsQueryQuery(request.DiplomaId), ct);
                if (!diplomaExsist.Success)
                {
                    result = RequestResponse<Unit>.Fail("Diploma does not exist.", 404);
                    return;
                }
                var isEnrolled = await _mediator.Send(
                    new CheckIfStudentIsAlreadyEnrolledInDiplomaQuery(studentId, request.DiplomaId), ct);

                if (isEnrolled.Success)
                {
                    result = RequestResponse<Unit>.Fail(isEnrolled.Message, isEnrolled.StatusCode);
                    return;
                }
                var enrollResponse = await _mediator.Send(new EnrollStudentInDiplomaCommand(studentId, request.DiplomaId), ct);

                if (!enrollResponse.Success)
                {
                    result = RequestResponse<Unit>.Fail("Enrollment failed.");
                    return;
                }
                result = RequestResponse<Unit>.Ok(Unit.Value, "Enrollment successful.");
            }, cancellationToken);

            return result!;
        }
    }
}
