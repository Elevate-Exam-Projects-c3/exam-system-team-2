using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Queries
{
    public record StudentDetailsQuery(Guid StudentId) : IRequest<RequestResponse<StudentDto>>;
}
