using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Handlers
{
    public class GetChickQuizIsPublishQueryHandler : IRequestHandler<GetChickQuizIsPublishQuery, RequestResponse<bool>>
    {
        private readonly IGenericRepository<Quiz> quizRepository;

        public GetChickQuizIsPublishQueryHandler(IGenericRepository<Quiz> quizRepository)
        {
            this.quizRepository = quizRepository;
        }
        public async Task<RequestResponse<bool>> Handle(GetChickQuizIsPublishQuery request, CancellationToken cancellationToken)
        {
            var quiz = quizRepository.Get(q => q.Id == request.QuizId).FirstOrDefault(c => c.Status == Common.Enums.QuizStatus.Published);

            if (quiz is not null)
            {
                return RequestResponse<bool>.Ok(true, "Quiz is published");
            }
            return RequestResponse<bool>.Fail("Quiz is not published", false, 404);
        }
    }
}
