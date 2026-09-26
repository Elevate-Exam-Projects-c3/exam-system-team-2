using exam_system.Domain.Entities.Identity;
using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers
{
    public class StudentDetailsQueryHandler:IRequestHandler<StudentDetailsQuery, RequestResponse<StudentDto>>
    {
        private readonly IGenericRepository<Student> _studentRepo;
        public StudentDetailsQueryHandler(IGenericRepository<Student> studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public async Task<RequestResponse<StudentDto>> Handle(StudentDetailsQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetAll()
                .Where(s => s.Id == request.StudentId)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FullName = s.User.FullName,
                }).FirstOrDefaultAsync(cancellationToken);

            if(student==null)
                return RequestResponse<StudentDto>.Fail("student not found.");

            return RequestResponse<StudentDto>.Ok(student, "Successfully retrieved student information.");
        }
    }
}
