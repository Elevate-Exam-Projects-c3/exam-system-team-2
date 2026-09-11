using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators.OrchestratorHandlers
{
    public class AddQuestionWithOptionsOrchestratorHandler : IRequestHandler<AddQuestionWithOptionsOrchestrator,
            RequestResponse<Guid>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionWithOptionsOrchestratorHandler(IMediator mediator , IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<Guid>> Handle(AddQuestionWithOptionsOrchestrator request, CancellationToken cancellationToken)
        {

            //1. Quiz Validation
            var quiz = await _mediator.Send( new GetQuizByIdQuery(request.QuizId),cancellationToken);

            if (quiz is null)
            {
                throw new KeyNotFoundException( $"Quiz with ID {request.QuizId} was not found.");
            }


            //2. Action Validation : Options Count must be >= 2 
            if (request.Options.Count < 2)
            {
                throw new InvalidOperationException( "A question must have at least two options.");
            }

            //3. Options Count must be isCorrect = true for only one option :
            if (request.Options.Count(o => o.IsCorrect) != 1)
            {
                throw new InvalidOperationException( "A question must have exactly one correct option.");
            }

            //4. Create Question :
            var questionResult = await _mediator.Send(new CreateQuestionCommand(request.QuizId, request.Text, request.Explanation),cancellationToken);

            //5. Get QuestionId from Result : 
            if (questionResult.Data == null)
            {
                return RequestResponse<Guid>.Fail("Question creation did not return a QuestionId.");
            }
            var questionId = questionResult.Data;

            //6. Create Options:
            await _mediator.Send(new CreateOptionsCommand(questionId, request.Options), cancellationToken);

            //7. Save Changes :
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //8. Return Response :
            return RequestResponse<Guid>.Created(questionId, "Question and options created successfully.");
        }
    }
}
