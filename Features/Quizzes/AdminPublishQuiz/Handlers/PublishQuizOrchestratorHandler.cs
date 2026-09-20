using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers
{
    public class PublishQuizOrchestratorHandler : IRequestHandler<PublishQuizOrchestrator, RequestResponse<PublishQuizResult>>
    {
        private readonly IMediator _mediator;

        public PublishQuizOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResponse<PublishQuizResult>> Handle(
            PublishQuizOrchestrator request,
            CancellationToken cancellationToken)
        {
            // 1. Check if the quiz is ready to be published
            var readinessResult = await _mediator.Send(new GetQuizPublishReadinessQuery(request.QuizId),cancellationToken);

            // 2. If the readiness check failed or quiz was not found
            if (!readinessResult.Success || readinessResult.Data == null)
            {
                return RequestResponse<PublishQuizResult>.Fail(
                    readinessResult.Message,
                    readinessResult.StatusCode);
            }

            // 3. Build the publish result from readiness data
            var result = new PublishQuizResult(
                readinessResult.Data.IsReadyToPublish,
                readinessResult.Data.Checks);

            // 4. Stop if the quiz is not ready
            if (!result.IsReadyToPublish)
            {
                return RequestResponse<PublishQuizResult>.Fail(
                    "Quiz is not ready to be published. Please fix the failing checks.", result,400);
            }

            // 5. Publish the quiz
            var publishResult = await _mediator.Send(new PublishQuizCommand(request.QuizId),cancellationToken);

            // 6. If publishing failed
            if (!publishResult.Success)
            {
                return RequestResponse<PublishQuizResult>.Fail(
                    publishResult.Message,
                    publishResult.StatusCode);
            }

            // 7. Return successful result
            return RequestResponse<PublishQuizResult>.Ok(result,"Quiz published successfully.",200);
        }
    }
}
