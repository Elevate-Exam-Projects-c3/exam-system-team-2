using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollStudentInDiplomaCommandHandler : IRequestHandler<EnrollStudentInDiplomaCommand, RequestResponse<Unit>>
    {

        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public EnrollStudentInDiplomaCommandHandler(IGenericRepository<StudentEnrollment> enrollmentRepo, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _enrollmentRepo = enrollmentRepo;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<Unit>> Handle(EnrollStudentInDiplomaCommand request, CancellationToken cancellationToken)
        {

            var existingEnrollment = await _mediator.Send(new CheckIfStudentIsAlreadyEnrolledInDiplomaQuery(request.StudentId, request.DiplomaId));

            if (existingEnrollment.Success)
                return RequestResponse<Unit>.Fail("User is already enrolled in the diploma.");

            var enrollment = new StudentEnrollment
            {
                StudentId = request.StudentId,
                DiplomaId = request.DiplomaId,
            };

            await _enrollmentRepo.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse<Unit>.Ok(Unit.Value, "Enrollment successful.");

        }

    }
}
