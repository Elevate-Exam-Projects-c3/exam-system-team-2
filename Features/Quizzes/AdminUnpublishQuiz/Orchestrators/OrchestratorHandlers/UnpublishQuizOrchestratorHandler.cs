using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators.OrchestratorHandlers
{
    public class UnpublishQuizOrchestratorHandler : IRequestHandler<UnpublishQuizOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;

        public UnpublishQuizOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse> Handle(UnpublishQuizOrchestrator request, CancellationToken cancellationToken)
        {
            //1. Count in-progress attempts on this quiz :
            var attemptsCountResult = await _mediator.Send(new CountInProgressQuizAttemptsQuery(request.QuizId), cancellationToken);
           
            //check if the count query was successful :
            if (!attemptsCountResult.Success)
            {
                return RequestResponse.Fail(attemptsCountResult.Message, attemptsCountResult.StatusCode);
            }

            //2. if any in-progress attempts exist, reject with 409 : 
            if (attemptsCountResult.Data > 0)
            {
                return RequestResponse.Fail("Cannot unpublish this quiz while students have in-progress attempts.", 409);
            }

            //3. Execute the unpublish operation :
            var unpublishResult = await _mediator.Send(new UnpublishQuizCommand(request.QuizId), cancellationToken);

            return unpublishResult;
        }
    }
}
