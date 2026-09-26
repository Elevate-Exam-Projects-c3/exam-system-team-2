using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Queries
{
    public record StudentEnrolledDiplomasQuery(Guid studentId) : IRequest<RequestResponse<IEnumerable<DiplomaStudentDashboardDto>>>;
}
