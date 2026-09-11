using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers
{
    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand, RequestResponse<PublishQuizResult>>
    {
        private readonly IMediator _mediator;
        private readonly IGenericRepository<Quiz> _quizRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PublishQuizCommandHandler(IMediator mediator, IGenericRepository<Quiz> quizRepository, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<PublishQuizResult>> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            //1. Check if the quiz is ready to be published :
            var quizReadinessResult = await _mediator.Send(new GetQuizPublishReadinessQuery(request.QuizId),cancellationToken);

            //2. Check if the readiness query failed or the quiz was not found:
            if (!quizReadinessResult.Success || quizReadinessResult.Data == null)
            {
                return RequestResponse<PublishQuizResult>.Fail(quizReadinessResult.Message, quizReadinessResult.StatusCode);
            }

            //Mapping result Query to result Command :  
            var result = new PublishQuizResult(quizReadinessResult.Data.IsReadyToPublish, quizReadinessResult.Data.Checks);
            //3. Check if the quiz is ready to be published :
            if (!result.IsReadyToPublish)
            {
                return RequestResponse<PublishQuizResult>.Fail(
                    "Quiz is not ready to be published. Please fix the failing checks.", result, 400);
            }

            //4.Get the quiz from the repository and check if it exists :
            var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
            if (quiz == null)
            {
                return RequestResponse<PublishQuizResult>.Fail("Quiz not found.", 404);
            }

            quiz.Status = QuizStatus.Published;
            quiz.PublishedAt = DateTime.UtcNow;
            _quizRepository.Update(quiz);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse<PublishQuizResult>.Ok(result, "Quiz published successfully.", 200);

        }
    }
}
