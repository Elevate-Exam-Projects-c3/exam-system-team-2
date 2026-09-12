using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler:IRequestHandler<CheckIfStudentIsAlreadyEnrolledInDiplomaQuery, RequestResponse<bool>>
    {
        #region Fields
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        #endregion
        #region Constructor
        public CheckIfStudentIsAlreadyEnrolledInDiplomaQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        
        #endregion
        #region Handle Operation
        public async Task<RequestResponse<bool>> Handle(CheckIfStudentIsAlreadyEnrolledInDiplomaQuery request, CancellationToken cancellationToken)
        {
            var existingEnrollment = await _enrollmentRepo.GetAll()
                .Where(se => se.StudentId == request.studentId && se.DiplomaId == request.diplomaId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if( existingEnrollment != null)
                return RequestResponse<bool>.Ok(true, "User is already enrolled in the diploma.");

            return RequestResponse<bool>.Ok(false, "User is not enrolled in the diploma.");
        }
        #endregion
    }
}
