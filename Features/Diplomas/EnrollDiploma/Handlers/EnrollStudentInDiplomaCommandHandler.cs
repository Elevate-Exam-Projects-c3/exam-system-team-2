using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Diplomas.GetDiplomaForEnrollment.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollStudentInDiplomaCommandHandler:IRequestHandler<EnrollStudentInDiplomaCommand, RequestResponse<Unit>>
    {

        #region Fields       
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public EnrollStudentInDiplomaCommandHandler(IGenericRepository<StudentEnrollment> enrollmentRepo, IUnitOfWork unitOfWork)
        {
            _enrollmentRepo = enrollmentRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Handle Operation
        public async Task<RequestResponse<Unit>> Handle(EnrollStudentInDiplomaCommand request, CancellationToken cancellationToken)
        {
            var existingEnrollment = await _enrollmentRepo.GetAll()
               .Where(se => se.StudentId == request.StudentId && se.DiplomaId == request.DiplomaId)
               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existingEnrollment != null)
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
        #endregion

    }
}
