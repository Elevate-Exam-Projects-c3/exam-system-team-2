using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers
{
    public class CountInProgressQuizAttemptsQueryHandler : IRequestHandler<CountInProgressQuizAttemptsQuery, RequestResponse<int>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public CountInProgressQuizAttemptsQueryHandler(
            IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }
        public async Task<RequestResponse<int>> Handle(CountInProgressQuizAttemptsQuery request, CancellationToken cancellationToken)
        {
            //1. check Quiz Validation :
            if (request.QuizId == Guid.Empty)
            {
                return RequestResponse<int>.Fail("QuizId is required", statusCode: 400);
            }

            //2.Count In Progress Quiz Attempts :
            var count = await _attemptRepository.CountAsync(
                attempt => attempt.QuizId == request.QuizId && 
                attempt.Status == AttemptStatus.InProgress &&
               !attempt.IsDeleted);

            return RequestResponse<int>.Ok(count, "In-progress attempts counted successfully.");
        }
    }
}
