using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Commands
{
    //Temporary Command :
    public record AutoTimeoutAttemptCommand(Guid AttemptId) : IRequest<RequestResponse>;
}
