using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries
{
    public record GetQuizPublishReadinessQuery(Guid QuizId) : IRequest<RequestResponse<QuizPublishReadinessDto>>;

}
