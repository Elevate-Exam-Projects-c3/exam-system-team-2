using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using System.Security.Claims;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Features.Diplomas.GetDiplomaForEnrollment.Queries;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;


namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollDiplomaOrchestratorHandler:IRequestHandler<EnrollDiplomaOrchestrator,RequestResponse<Unit>>
    {

        #region Fields       
        private readonly IMediator _mediator;
        private readonly ICurrentUserId _currentUserId;

        #endregion

        #region Constructor
        public EnrollDiplomaOrchestratorHandler(IMediator mediator, ICurrentUserId currentUserId)
        {
            _mediator = mediator;
            _currentUserId = currentUserId;
        }
        #endregion

        #region Handle Operation
        public async Task<RequestResponse<Unit>> Handle(EnrollDiplomaOrchestrator request, CancellationToken cancellationToken)
        {
            var userId =  _currentUserId.GetUserId();
            if(userId == null)
                return RequestResponse<Unit>.Fail("User is not authenticated.");

            var response = await _mediator.Send(new CheckIfDiplomaExistsQueryQuery(request.DiplomaId), cancellationToken);
            if (response.Success == false)
                return RequestResponse<Unit>.Fail("Diploma does not exist.");


            var isEnrolled = await _mediator.Send(
                new CheckIfStudentIsAlreadyEnrolledInDiplomaQuery(userId.Value, request.DiplomaId), cancellationToken);

            if (isEnrolled.Success == true)
                return RequestResponse<Unit>.Fail("Student is already enrolled in the diploma.");
           
            var enrollResponse = await _mediator.Send(new EnrollStudentInDiplomaCommand(userId.Value, request.DiplomaId), cancellationToken);
            if (enrollResponse.Success == false)
                return RequestResponse<Unit>.Fail("Enrollment failed.");

            return RequestResponse<Unit>.Ok(Unit.Value, "Enrollment successful.");
        }
        #endregion


    }
}
