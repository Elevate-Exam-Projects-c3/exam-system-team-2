using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers
{
    public class StudentQuizAttemptsQueryHandler:IRequestHandler<StudentQuizAttemptsQuery, RequestResponse<IEnumerable<RecentQuizAttemptDto>>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepo;

        public StudentQuizAttemptsQueryHandler(IGenericRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }
        public async Task<RequestResponse<IEnumerable<RecentQuizAttemptDto>>> Handle(StudentQuizAttemptsQuery request , CancellationToken cancellationToken)
        {
            var latestAttemptIds = await _attemptRepo.GetAll()
                .Where(a => a.StudentId == request.StudentId &&
                            a.SubmittedAt != null)
                .GroupBy(a => a.QuizId)
                .Select(g => g
                    .OrderByDescending(a => a.SubmittedAt)
                    .ThenByDescending(a => a.Id)
                    .Select(a => a.Id)
                    .First())
                .ToListAsync(cancellationToken);
            var attempts = await _attemptRepo.GetAll()
                .Where(a=>latestAttemptIds.Contains(a.Id))
                .Select(a => new RecentQuizAttemptDto
                {
                    QuizId = a.QuizId,
                    QuizTitle = a.Quiz.Title,

                    Score = a.Score,
                    Passed = a.Passed,

                    StartTime = a.StartTime,
                    SubmittedAt = a.SubmittedAt!.Value,

                    CorrectAnswersCount = a.Answers.Count(x => x.IsCorrect == true)
                })
                .OrderByDescending(a => a.SubmittedAt)
                .ToListAsync(cancellationToken);
            
            return RequestResponse<IEnumerable<RecentQuizAttemptDto>>
                .Ok(attempts, "Student quiz attempts retrieved successfully.");
        }
    }
}
