using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Orchestrators;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using MediatR;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers
{
    public class GetDiplomaDetailOrchestratorHandler : IRequestHandler<GetDiplomaDetailOrchestrator, RequestResponse<ViewDiplomaDetailsDto>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserId _currentUserId;
        public GetDiplomaDetailOrchestratorHandler(IMediator mediator, ICurrentUserId currentUserId)
        {
            _mediator = mediator;
            _currentUserId = currentUserId;
        }
        public async Task<RequestResponse<ViewDiplomaDetailsDto>> Handle(GetDiplomaDetailOrchestrator request, CancellationToken cancellationToken)
        {
            var studentId = _currentUserId.GetStudentId();
            //var studentId = Guid.Parse("45D11C22-72EE-497D-B5EF-1E1D51D8DC0D");
            if (studentId == null)
                return RequestResponse<ViewDiplomaDetailsDto>
                    .Fail("Unauthorized.", 401);

            var diplomaQuery = await _mediator.Send(
                new GetDiplomaDetailByIdQuery(request.DiplomaId),
                cancellationToken);

            if (!diplomaQuery.Success || diplomaQuery.Data == null)
                return RequestResponse<ViewDiplomaDetailsDto>
                    .Fail(diplomaQuery.Message, 404);

            var quizQuery = await _mediator.Send(
                new GetDiplomaQuizzesQuery(request.DiplomaId),
                cancellationToken);

            if (!quizQuery.Success || quizQuery.Data == null)
                return RequestResponse<ViewDiplomaDetailsDto>
                    .Fail(quizQuery.Message, 404);


            var diplomaDto = diplomaQuery.Data;
            var diplomaQuizzes = quizQuery.Data.ToList(); //why here worning nullable

            var quizIds = diplomaQuizzes
                .Select(q => q.Id)
                .ToList();

            var attemptQuery = await _mediator.Send(
                new GetStudentAttemptsQuery(studentId.Value, quizIds),
                cancellationToken);

            if (!attemptQuery.Success || attemptQuery.Data == null)
                return RequestResponse<ViewDiplomaDetailsDto>.Fail(attemptQuery.Message);

            var studentAttempts = attemptQuery.Data
                .ToDictionary(a => a.QuizId);

            var quizzes = diplomaQuizzes.Select(quiz =>
            {
                studentAttempts.TryGetValue(
                    quiz.Id,
                    out var studentAttempt);

                var isResumable = studentAttempt?.IsResumable ?? false;
                var attemptCount = studentAttempt?.AttemptCount ?? 0;

                bool canStudentAttempt;

                if (isResumable)
                {
                    canStudentAttempt = false;
                }
                else
                {
                    canStudentAttempt = attemptCount < quiz.MaxAttempts;
                }

                return new DiplomaQuizDetailsDto
                {
                    Quiz = quiz,
                    StudentAttempt = new StudentAttemptDto
                    {
                        QuizId = quiz.Id,
                        AttemptCount = attemptCount,
                        IsResumable = isResumable,
                        CanStudentAttempt = canStudentAttempt
                    }
                };
            }).ToList();

            var diplomaResult = new ViewDiplomaDetailsDto
            {
                Id = diplomaDto.Id,
                Title = diplomaDto.Title,
                Description = diplomaDto.Description,
                ImageUrl = diplomaDto.ImageUrl,
                Quizzes = quizzes
            };

            return RequestResponse<ViewDiplomaDetailsDto>.Ok(
                diplomaResult,
                "Diploma retrieved successfully.");

        }
    }
}
