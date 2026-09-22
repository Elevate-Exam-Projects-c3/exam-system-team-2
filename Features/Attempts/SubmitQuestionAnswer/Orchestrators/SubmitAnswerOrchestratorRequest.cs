using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Orchestrators
{
    public record SubmitAnswerOrchestratorRequest(
    Guid AttemptId,
    Guid QuestionId,
    Guid? SelectedOptionId,
    Guid CallerUserId
) : IRequest<RequestResponse>;
}
