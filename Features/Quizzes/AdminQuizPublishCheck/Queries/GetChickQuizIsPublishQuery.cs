using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries
{
    public record GetChickQuizIsPublishQuery(Guid QuizId) : IRequest<RequestResponse<bool>>;

}
