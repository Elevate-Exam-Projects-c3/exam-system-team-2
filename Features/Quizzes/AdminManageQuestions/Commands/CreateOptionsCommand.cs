using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands
{
    public record CreateOptionsCommand(Guid QuestionId, List<OptionItem> Options) : IRequest<RequestResponse>;
}
