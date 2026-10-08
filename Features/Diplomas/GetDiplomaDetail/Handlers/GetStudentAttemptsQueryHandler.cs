using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers
{
    public class GetStudentAttemptsQueryHandler : IRequestHandler<GetStudentAttemptsQuery, RequestResponse<IEnumerable<StudentAttemptDto>>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepo;
        public GetStudentAttemptsQueryHandler(IGenericRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResponse<IEnumerable<StudentAttemptDto>>> Handle(GetStudentAttemptsQuery request, CancellationToken cancellationToken)
        {
            var studentAttempts = await _attemptRepo.GetAll()
                .AsNoTracking()
                .Where(a => a.StudentId == request.StudentId &&
                request.QuizIds.Contains(a.QuizId))
                .GroupBy(a => a.QuizId)
                .Select(g => new StudentAttemptDto
                {
                    QuizId = g.Key,
                    AttemptCount = g.Count(),
                    IsResumable = g.Any(a => a.Status == AttemptStatus.InProgress)
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<StudentAttemptDto>>.Ok(
                studentAttempts,
                "Student attempts retrieved successfully.");
        }
    }
}
