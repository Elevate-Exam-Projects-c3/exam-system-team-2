using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using System.Security.Claims;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;


namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollDiplomaOrchestratorHandler:IRequestHandler<EnrollDiplomaOrchestrator,RequestResponse<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserId _currentUserId;
        public EnrollDiplomaOrchestratorHandler(IMediator mediator, ICurrentUserId currentUserId)
        {
            _mediator = mediator;
            _currentUserId = currentUserId;
        }

        public async Task<RequestResponse<Unit>> Handle(EnrollDiplomaOrchestrator request, CancellationToken cancellationToken)
        {
            var studentId = _currentUserId.GetStudentId() ?? _currentUserId.GetUserId();
            if (studentId == null)
                return RequestResponse<Unit>.Fail("User is not authenticated.", 401);

            var diplomaExsist = await _mediator.Send(new CheckIfDiplomaExistsQueryQuery(request.DiplomaId), cancellationToken);
            if (!diplomaExsist.Success)
                return RequestResponse<Unit>.Fail("Diploma does not exist.", 404);

            var isEnrolled = await _mediator.Send(
                new CheckIfStudentIsAlreadyEnrolledInDiplomaQuery(studentId.Value, request.DiplomaId), cancellationToken);

            if (isEnrolled.Success)
                return RequestResponse<Unit>.Fail(isEnrolled.Message, isEnrolled.StatusCode);
           
            var enrollResponse = await _mediator.Send(new EnrollStudentInDiplomaCommand(studentId.Value, request.DiplomaId), cancellationToken);

            if (!enrollResponse.Success)
                return RequestResponse<Unit>.Fail("Enrollment failed.");

            return RequestResponse<Unit>.Ok(Unit.Value, "Enrollment successful.");
        }
    }
}
