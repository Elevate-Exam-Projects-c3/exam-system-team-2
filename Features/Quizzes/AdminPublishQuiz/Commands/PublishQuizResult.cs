using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Commands
{
    public record PublishQuizResult(bool IsReadyToPublish, List<PublishCheckItemDto> Checks);
}
