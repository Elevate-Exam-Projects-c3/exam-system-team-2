using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers
{
    public class CreateOptionsCommandHandler : IRequestHandler<CreateOptionsCommand, RequestResponse>
    {
        private readonly IGenericRepository<QuestionOption> _questionOptionRepo;

        public CreateOptionsCommandHandler(IGenericRepository<QuestionOption> questionOptionRepo)
        {
            _questionOptionRepo = questionOptionRepo;
        }
        public async Task<RequestResponse> Handle(CreateOptionsCommand request, CancellationToken cancellationToken)
        {
            

            //1. Validate Options Count and Correctness :
            if (request.Options.Count < 2)
            {
                throw new InvalidOperationException("A question must have at least two options.");
            }

            if (request.Options.Count(o => o.IsCorrect) != 1)
            {
                throw new InvalidOperationException("A question must have exactly one correct option.");
            }

            //2. Map OptionItems to QuestionOptions : 
            var options = request.Options.Select(option => new QuestionOption
            {
                QuestionId = request.QuestionId,
                OptionText = option.OptionText,
                IsCorrect = option.IsCorrect
            }).ToList();

            //3. Add Options : 
            await _questionOptionRepo.AddRangeAsync(options);

            //4. Return Success Response :
            return RequestResponse.Ok("Options created successfully.");
        }
    }
}
