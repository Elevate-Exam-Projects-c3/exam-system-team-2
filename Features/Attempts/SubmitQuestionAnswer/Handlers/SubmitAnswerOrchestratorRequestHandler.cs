using exam_system.Common.Enums;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Orchestrators;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers
{
    public class SubmitAnswerOrchestratorRequestHandler : IRequestHandler<SubmitAnswerOrchestratorRequest, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitAnswerOrchestratorRequestHandler(IMediator mediator , IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(SubmitAnswerOrchestratorRequest request, CancellationToken cancellationToken)
        {
            // 1. Get attempt information :
            var attemptResult = await _mediator.Send(new GetAttemptForAnswerQuery(request.AttemptId), cancellationToken);

            if (!attemptResult.Success)
            {
                return RequestResponse.Fail(
                    attemptResult.Message,
                    attemptResult.StatusCode,
                    attemptResult.Errors);
            }

            //Get Attempt Data :
            var attempt = attemptResult.Data!;

            //2. Ownership Check :
            if (attempt.StudentUserId != request.CallerUserId)
            {
                return RequestResponse.Fail("Attempt not found.", 404);   
            }

            //3. Make sure the attempt is still in progress
            if (attempt.Status != AttemptStatus.InProgress)
            {
                return RequestResponse.Fail("Attempt is no longer in progress.", 400);
            }

            // TEMPORARY: Placeholder auto-timeout logic for EXAM-132.
            // This will be replaced by the shared Auto-Submit logic from EXAM-135
            // once EXAM-135 is completed.
            if (DateTime.UtcNow > attempt.Deadline)
            {
                var timeoutResult = await _mediator.Send(new AutoTimeoutAttemptCommand(request.AttemptId),cancellationToken);
                if (!timeoutResult.Success)
                {
                    return timeoutResult;
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return RequestResponse.Fail("The attempt time has expired. The attempt has been auto-submitted.",410);
            }



            //5. Get the question is a part of the quiz :
            var questionResult = await _mediator.Send( new ValidateQuestionForAttemptQuery( request.QuestionId,attempt.QuizId),cancellationToken);

            if (!questionResult.Success)
            {
                return RequestResponse.Fail(
                    questionResult.Message,
                    questionResult.StatusCode,
                    questionResult.Errors);
            }

            //If the question isn't part of the quiz :
            if (!questionResult.Data!.IsValid)
            {
                return RequestResponse.Fail("Question does not belong to the attempt's quiz.",400);
            }

            //6. Save / update the answer
            var answerResult = await _mediator.Send(
                new SubmitAnswerCommand(
                    request.AttemptId,
                    request.QuestionId,
                    request.SelectedOptionId),
                cancellationToken);

            if (!answerResult.Success)
            {
                return answerResult;
            }

            //7. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse.Ok("Answer saved successfully.");
        }
    }
}
