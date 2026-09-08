using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers
{
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Question> _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionCommandHandler(IGenericRepository<Question> questionRepository , IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<Guid>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {

            //1. Quize Validation: 
            var quiz = await _questionRepository.GetByIdAsync( request.QuizId);
            if (quiz is null)
            {
                throw new KeyNotFoundException(
                    $"Quiz with ID {request.QuizId} was not found.");
            }

            // 2. Calculate OrderIndex
            var orderIndex = await _questionRepository
                .CountAsync(q => q.QuizId == request.QuizId);

            // 3. Create Question
            var question = new Question
            {
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = orderIndex + 1
            };

            // 4. Add Question
            await _questionRepository.AddAsync(question);
           return RequestResponse<Guid>.Created(question.Id, "Question created successfully.");
        }


    }
}
