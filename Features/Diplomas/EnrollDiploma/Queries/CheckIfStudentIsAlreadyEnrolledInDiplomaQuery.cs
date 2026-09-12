using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Queries
{
    public record CheckIfStudentIsAlreadyEnrolledInDiplomaQuery(Guid studentId, Guid diplomaId) : IRequest<RequestResponse<bool>>;
}
