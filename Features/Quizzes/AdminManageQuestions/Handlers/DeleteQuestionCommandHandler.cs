using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Domain.Common;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using exam_system.Common.Enums;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, RequestResponse>
    {
        private readonly IGenericRepository<Question> _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(IGenericRepository<Question> questionRepository , IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            //1. Get Question With the Quize :
            var question = await _questionRepository.GetByIdAsync(request.QuestionId, q => q.Quiz);

            //2. Check of the Question Validation :
            if (question is null)
            {
                throw new KeyNotFoundException(
                    $"Question with ID {request.QuestionId} was not found.");
            }

            //3. Delete Guard : 
            if(question.Quiz.Status == QuizStatus.Published)
            {
                throw new InvalidOperationException(
                    "Cannot delete a question from a published quiz.");
            }

            //4. Soft Delete the Question :
            await _questionRepository.DeleteAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse.Ok("Question deleted successfully.");
        }
    }
}
