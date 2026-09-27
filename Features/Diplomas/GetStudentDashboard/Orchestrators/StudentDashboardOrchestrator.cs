using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Orchestrators
{
    public record StudentDashboardOrchestrator : IRequest<RequestResponse<StudentDashboardDto>>;
}
