using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers
{
    public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, QuizDto?>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;

        public GetQuizByIdQueryHandler(
            IGenericRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<QuizDto?> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            //1. Get Quize by Id :
            var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
            if (quiz == null)
            {
                return null;
            }
            //2. Map to QuizDto and return :
            return new QuizDto
            {
                Id = quiz.Id
            };

        }
    }
}
