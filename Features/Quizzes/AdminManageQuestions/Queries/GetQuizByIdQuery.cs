using exam_system.Features.Quizzes.AdminManageQuestions.Dtos;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries
{
    public record GetQuizByIdQuery(Guid QuizId) : IRequest<QuizDto?>;
}
