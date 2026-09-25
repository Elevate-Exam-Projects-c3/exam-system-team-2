using exam_system.Common.Enums;
using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Attempts.StartAttempt.Dtos;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Orchestrators
{
    public class StartAttemptQuizOrchestratorHandler : IRequestHandler<StartAttemptOrchestrator, RequestResponse<StartAttemptResponseDto>>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public StartAttemptQuizOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<StartAttemptResponseDto>> Handle(StartAttemptOrchestrator request, CancellationToken cancellationToken)
        {
            var quizPublishResult = await mediator.Send(new GetChickQuizIsPublishQuery(request.QuizId), cancellationToken);

            if (!quizPublishResult.Success)
            {
                return RequestResponse<StartAttemptResponseDto>.Fail(quizPublishResult.Message, quizPublishResult.StatusCode, quizPublishResult.Errors);
            }

            var GetStudentAttemptResult = await mediator.Send(new GetExistStudentAttemptQuery(request.QuizId, request.StudentId), cancellationToken);

            if (!GetStudentAttemptResult.Success)
            {
                return RequestResponse<StartAttemptResponseDto>.Fail(GetStudentAttemptResult.Message, GetStudentAttemptResult.StatusCode, GetStudentAttemptResult.Errors);
            }
            if(GetStudentAttemptResult.Data.Status == AttemptStatus.InProgress || GetStudentAttemptResult.Data.Status == AttemptStatus.TimedOut)
            {
                return RequestResponse<StartAttemptResponseDto>.Fail("Student has an in-progress or timed-out attempt for this quiz.", 400, null);
            }

            var CheckCountofAttempts = await mediator.Send(new GetChickRightToAttemptQuery(request.QuizId, request.StudentId), cancellationToken);

            if(CheckCountofAttempts < 1)
            {
                return RequestResponse<StartAttemptResponseDto>.Fail("Student has reached the maximum number of attempts for this quiz.", 400, null);
            }



        }
    }
}
