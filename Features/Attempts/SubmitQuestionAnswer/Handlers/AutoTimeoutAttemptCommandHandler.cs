using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers
{
    public class AutoTimeoutAttemptCommandHandler : IRequestHandler<AutoTimeoutAttemptCommand, RequestResponse>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public AutoTimeoutAttemptCommandHandler(IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }
        public async Task<RequestResponse> Handle(AutoTimeoutAttemptCommand request, CancellationToken cancellationToken)
        {

            var attempt = await _attemptRepository.Get(a => a.Id == request.AttemptId)
                                                  .FirstOrDefaultAsync(cancellationToken);

            if (attempt is null)
            {
                return RequestResponse.Fail("Attempt not found.", 404);
            }

            attempt.Status = AttemptStatus.TimedOut;
            attempt.SubmittedAt = attempt.Deadline;

            _attemptRepository.Update(attempt);

            return RequestResponse.Ok("Attempt timed out successfully.");

        }
    }
}
