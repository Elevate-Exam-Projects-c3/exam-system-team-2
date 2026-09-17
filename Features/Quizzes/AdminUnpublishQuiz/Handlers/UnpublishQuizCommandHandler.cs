using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers
{
    public class UnpublishQuizCommandHandler : IRequestHandler<UnpublishQuizCommand, RequestResponse>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishQuizCommandHandler(IGenericRepository<Quiz> quizRepository , IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
        {

            //1. Validate the request:
            if (request.QuizId == Guid.Empty)
            {
                return RequestResponse.Fail("QuizId is required.", 400);
            }

            //2. Get the quiz and check it exists :
            var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
            if (quiz == null)
            {
                return RequestResponse.Fail("Quiz not found.", 404);
            }

            //3. Check if the quiz is currently published:
            if (quiz.Status != QuizStatus.Published)
            {
                return RequestResponse.Fail("Quiz is not currently published.", 400);
            }

            //4. Unpublish the quiz:
            quiz.Status = QuizStatus.Draft;
            _quizRepository.Update(quiz);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.Ok("Quiz has been unpublished successfully.");
        }
    }
}
