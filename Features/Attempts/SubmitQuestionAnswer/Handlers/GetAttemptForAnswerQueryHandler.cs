using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Dtos;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers
{
    public class GetAttemptForAnswerQueryHandler : IRequestHandler<GetAttemptForAnswerQuery, RequestResponse<AttemptAnswerDto>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public GetAttemptForAnswerQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }
        public async Task<RequestResponse<AttemptAnswerDto>> Handle(GetAttemptForAnswerQuery request, CancellationToken cancellationToken)
        {

            //1.Get Data of Attempts :
            var attempt = await _attemptRepository.Get(a => a.Id == request.AttemptId)
                  .Select(a => new AttemptAnswerDto
                  {
                      AttemptId = a.Id,
                      QuizId = a.QuizId,
                      Status = a.Status,
                      Deadline = a.Deadline,
                      StudentUserId = a.Student.UserId
                  }).FirstOrDefaultAsync(cancellationToken);

            if (attempt is null)
            {
                return RequestResponse<AttemptAnswerDto>.Fail("Attempt not found", 404);
            }

            return RequestResponse<AttemptAnswerDto>.Ok(attempt);
        }
    }
}
