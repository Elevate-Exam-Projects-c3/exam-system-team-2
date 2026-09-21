using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Queries
{
    public record GetStudentAttemptsQuery(Guid StudentId, IEnumerable<Guid> QuizIds) : IRequest<RequestResponse<IEnumerable<StudentAttemptDto?>>>;
}