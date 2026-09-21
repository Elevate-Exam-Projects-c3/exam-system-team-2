using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Dtos;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers
{
    public class ValidateQuestionForAttemptQueryHandler : IRequestHandler<ValidateQuestionForAttemptQuery, RequestResponse<ValidateQuestionForAttemptDto>>
    {
        private readonly IGenericRepository<Question> _questionRepository;

        public ValidateQuestionForAttemptQueryHandler(
            IGenericRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<RequestResponse<ValidateQuestionForAttemptDto>> Handle(ValidateQuestionForAttemptQuery request, CancellationToken cancellationToken)
        {

            var exists = await _questionRepository
                   .Get(q => q.Id == request.QuestionId && q.QuizId == request.QuizId)
                   .AnyAsync(cancellationToken);

            var result = new ValidateQuestionForAttemptDto
            {
                IsValid = exists
            };

            return RequestResponse<ValidateQuestionForAttemptDto>.Ok(result);
        }
    }
}
