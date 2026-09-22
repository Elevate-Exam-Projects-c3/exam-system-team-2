using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler : IRequestHandler<CheckIfStudentIsAlreadyEnrolledInDiplomaQuery, RequestResponse<Unit>>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        public CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse<Unit>> Handle(CheckIfStudentIsAlreadyEnrolledInDiplomaQuery request, CancellationToken cancellationToken)
        {
            var existingEnrollment = await _enrollmentRepo
                .GetAll()
                .Where(se => se.StudentId == request.studentId && se.DiplomaId == request.diplomaId)
                .AnyAsync(cancellationToken);

            if (!existingEnrollment)
                return RequestResponse<Unit>.Fail("User is not enrolled in the diploma.");

            return RequestResponse<Unit>.Ok(Unit.Value, "Student is already enrolled in the diploma.");
        }
    }
}
