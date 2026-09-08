using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators.OrchestratorHandlers
{
    public class AddQuestionWithOptionsOrchestratorHandler : IRequestHandler<AddQuestionWithOptionsOrchestrator,
            RequestResponse<AddQuestionWithOptionsResponseDto>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionWithOptionsOrchestratorHandler(IMediator mediator , IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<AddQuestionWithOptionsResponseDto>> Handle(AddQuestionWithOptionsOrchestrator request, CancellationToken cancellationToken)
        {
            //1. Action Validation : Options Count must be >= 2 
            if (request.Options.Count < 2)
            {
                throw new InvalidOperationException( "A question must have at least two options.");
            }

            //2. Options Count must be isCorrect = true for only one option :
            if (request.Options.Count(o => o.IsCorrect) != 1)
            {
                throw new InvalidOperationException( "A question must have exactly one correct option.");
            }

            //3. Create Question :
            var questionResult = await _mediator.Send(new CreateQuestionCommand(request.QuizId, request.Text, request.Explanation),cancellationToken);

            // 4. Get QuestionId from Result : 
            if (questionResult.Data == null)
            {
                throw new InvalidOperationException(
                    "Question creation did not return a QuestionId.");
            }
            var questionId = questionResult.Data;

            // 5. Create Options:
            await _mediator.Send(new CreateOptionsCommand(questionId, request.Options), cancellationToken);

            // 6. Save Changes :
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Return Response :

            var response = new AddQuestionWithOptionsResponseDto
            {
                QuestionId = questionId
            };

            return RequestResponse<AddQuestionWithOptionsResponseDto>
                .Created(  response,"Question and options created successfully.");
        }
    }
}
