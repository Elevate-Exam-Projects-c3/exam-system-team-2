using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries
{
    public record CountInProgressQuizAttemptsQuery(Guid QuizId) : IRequest<RequestResponse<int>>;

}
