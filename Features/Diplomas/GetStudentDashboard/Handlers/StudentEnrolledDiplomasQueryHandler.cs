using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers
{
    public class StudentEnrolledDiplomasQueryHandler:IRequestHandler<StudentEnrolledDiplomasQuery, RequestResponse<IEnumerable<DiplomaStudentDashboardDto>>>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepo;
        public StudentEnrolledDiplomasQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        public async Task<RequestResponse<IEnumerable<DiplomaStudentDashboardDto>>>Handle (StudentEnrolledDiplomasQuery request , CancellationToken cancellationToken)
        {
            var diplomas = await _enrollmentRepo.GetAll()
                .Where(d => d.StudentId == request.studentId)
                  .Select(e => new DiplomaStudentDashboardDto
                  {
                      Id = e.Diploma.Id,
                      Title = e.Diploma.Title,
                      Description = e.Diploma.Description,
                      ImageUrl = e.Diploma.ImageUrl,

                      // Total number of quizzes inside this diploma.
                      TotalQuizzesCount = e.Diploma.Quizzes.Count(),

                      TakenQuizzesCount = e.Diploma.Quizzes.Count(q => 
                        q.Attempts.Any(a => 
                            a.StudentId == request.studentId&&
                            a.SubmittedAt != null))
                  })
                .ToListAsync(cancellationToken);


            return RequestResponse<IEnumerable<DiplomaStudentDashboardDto>>
                .Ok(diplomas,"Student enrollment diplomas.");
        }
    }
}
