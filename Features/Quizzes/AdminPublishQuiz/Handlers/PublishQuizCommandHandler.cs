using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers
{
    public class PublishQuizCommandHandler
       : IRequestHandler<PublishQuizCommand, RequestResponse>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PublishQuizCommandHandler(
            IGenericRepository<Quiz> quizRepository,
            IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(PublishQuizCommand request,CancellationToken cancellationToken)
        {
            var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

            if (quiz == null)
            {
                return RequestResponse.Fail("Quiz not found.", 404);
            }

            quiz.Status = QuizStatus.Published;
            quiz.PublishedAt = DateTime.UtcNow;

            _quizRepository.Update(quiz);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse.Ok("Quiz published successfully.",200);
        }
    }
}
