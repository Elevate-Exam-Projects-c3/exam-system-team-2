using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptHistory.DTOs;
using exam_system.Features.Attempts.GetAttemptHistory.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptHistory.Handlers
{
    public class GetStudentAttemptHistoryQueryHandler:IRequestHandler<GetStudentAttemptHistoryQuery,RequestResponse<PaginatedResult<StudentAttemptDto>>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepo;
        private readonly ICurrentUserId _currentUserId;
        public GetStudentAttemptHistoryQueryHandler(IGenericRepository<QuizAttempt> attemptRepo,ICurrentUserId currentUserId)
        {
            _attemptRepo = attemptRepo;
            _currentUserId = currentUserId;
        }
        public async Task<RequestResponse<PaginatedResult<StudentAttemptDto>>> Handle (GetStudentAttemptHistoryQuery request, CancellationToken cancellationToken)
        {
            var studentId = _currentUserId.GetStudentId();
            //var studentId = Guid.Parse("AAAAAAAA-1111-1111-1111-AAAAAAAAAAAA");
            if (!studentId.HasValue)
                    return RequestResponse< PaginatedResult<StudentAttemptDto>>.Fail("Student not authenticated.");

                var totalCount = await _attemptRepo.GetAll()
                .CountAsync(a => a.StudentId == studentId.Value, cancellationToken);

            var skip = (request.PageNumber - 1) * request.PageSize;

            var studentAttempts = await _attemptRepo
                .GetAll()
                .Where(a => a.StudentId == studentId.Value)
                .OrderByDescending(a => a.StartTime)
                .Skip(skip)
                .Take(request.PageSize)
                .Select(attempt => new StudentAttemptDto
                {
                    QuizTitle = attempt.Quiz.Title,
                    Status = attempt.Status,Score = attempt.Score,
                    SubmittedAt = attempt.SubmittedAt,
                    QuizQuestionsCount = attempt.Quiz.Questions.Count(),
                    CorrectAnswerCount = attempt.Answers.Count(a => a.IsCorrect == true),
                    QuizDurationMinutes = attempt.Quiz.DurationMinutes,
                    AttemptTakeTimeInMin = attempt.SubmittedAt.HasValue? 
                        (int)(attempt.SubmittedAt.Value - attempt.StartTime).TotalMinutes: null
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var paginatedResult = PaginatedResult<StudentAttemptDto>.Create(
                studentAttempts,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return RequestResponse<PaginatedResult<StudentAttemptDto>>
                .Ok(
                    paginatedResult,
                    "Student attempt history retrieved successfully.");
        }
    }
}
