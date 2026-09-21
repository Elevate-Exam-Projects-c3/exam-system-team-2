using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class CheckDiplomaHasEnrollmentsQueryHandler : IRequestHandler<CheckDiplomaHasEnrollmentsQuery, RequestResponse<Unit>>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        public CheckDiplomaHasEnrollmentsQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        public async Task<RequestResponse<Unit>> Handle(CheckDiplomaHasEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var hasEnrollment = await _enrollmentRepo
                .GetAll()
                .Where(e => e.DiplomaId == request.DiplomaId)
                .AnyAsync(cancellationToken);

            if (!hasEnrollment)
                return RequestResponse<Unit>.Fail("Diploma has not Enrollments.");

            return RequestResponse<Unit>.Ok(Unit.Value, "Diploma has Enrollments.");
        }
    }
}
