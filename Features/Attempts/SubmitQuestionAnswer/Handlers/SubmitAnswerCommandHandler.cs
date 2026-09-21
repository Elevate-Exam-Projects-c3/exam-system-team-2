using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers
{
    public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, RequestResponse>
    {
        private readonly IGenericRepository<StudentQuestionAnswer> _answerRepository;

        public SubmitAnswerCommandHandler(IGenericRepository<StudentQuestionAnswer> answerRepository)
        {
            _answerRepository = answerRepository;
        }
        public async Task<RequestResponse> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {

            //1. Check if the answer already exists for the given attempt and question :
            var existingAnswer = await _answerRepository
                  .Get(a => a.AttemptId == request.AttemptId && a.QuestionId == request.QuestionId)
                  .FirstOrDefaultAsync(cancellationToken);

            //2. If it exists, update the selected option, otherwise create a new answer record:
            if (existingAnswer is not null)
            {
                existingAnswer.SelectedOptionId = request.SelectedOptionId;

                _answerRepository.Update(existingAnswer);
            }
            else
            {
                var answer = new StudentQuestionAnswer
                {
                    AttemptId = request.AttemptId,
                    QuestionId = request.QuestionId,
                    SelectedOptionId = request.SelectedOptionId
                };

                await _answerRepository.AddAsync(answer);
            }
            
            return RequestResponse.Ok("Answer saved successfully");
        }
    }
}
