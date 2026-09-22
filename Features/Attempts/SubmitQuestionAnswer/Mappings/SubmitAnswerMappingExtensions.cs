using exam_system.Features.Attempts.SubmitQuestionAnswer.Orchestrators;
using exam_system.Features.Attempts.SubmitQuestionAnswer.ViewModels;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Mappings
{
    public static class SubmitAnswerMappingExtensions
    {
        public static SubmitAnswerOrchestratorRequest ToOrchestratorRequest(
            this SubmitAnswerViewModel viewModel, Guid attemptId, Guid callerUserId)
        {
            return new SubmitAnswerOrchestratorRequest(
                attemptId,
                viewModel.QuestionId,
                viewModel.SelectedOptionId,
                callerUserId);
        }
    }
}
