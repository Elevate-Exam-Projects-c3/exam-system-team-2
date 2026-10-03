using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler : IRequestHandler<CheckIfStudentIsAlreadyEnrolledInDiplomaQuery, RequestResponse<bool>>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        public CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse<bool>> Handle(CheckIfStudentIsAlreadyEnrolledInDiplomaQuery request, CancellationToken cancellationToken)
        {
            var existingEnrollment = await _enrollmentRepo.GetAll()
                .AnyAsync(se => se.StudentId == request.studentId && se.DiplomaId == request.diplomaId,cancellationToken);

            return RequestResponse<bool>.Ok(existingEnrollment);
        }
    }
}
