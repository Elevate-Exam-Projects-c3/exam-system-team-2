using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers
{
    public class GetAttemptCountQueryHandler : IRequestHandler<GetAttemptCountQuery, int>
    {
        private readonly IGenericRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IGenericRepository<Quiz> quizRepository;

        public GetAttemptCountQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepository , IGenericRepository<Quiz> QuizRepository)
        {
            _quizAttemptRepository = quizAttemptRepository;
            quizRepository = QuizRepository;
        }
        public Task<int> Handle(GetAttemptCountQuery request, CancellationToken cancellationToken)
        {
            var QuizMaxAttempts = quizRepository.Get(q => q.Id == request.QuizId).Select(x => x.MaxAttempts).FirstOrDefault();

            var StudentAttemptCount = _quizAttemptRepository.Get(q => q.QuizId == request.QuizId && q.StudentId == request.StudentId).Count();
        }
    }
}
