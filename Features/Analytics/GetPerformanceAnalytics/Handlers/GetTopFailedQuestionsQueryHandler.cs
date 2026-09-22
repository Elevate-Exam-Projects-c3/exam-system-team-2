using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Analytics.GetPerformanceAnalytics.Queries;
using exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Analytics.GetPerformanceAnalytics.Handlers;

public class GetTopFailedQuestionsQueryHandler : IRequestHandler<GetTopFailedQuestionsQuery, IReadOnlyList<FailedQuestionDto>>
{
    private readonly IGenericRepository<StudentQuestionAnswer> _answerRepository;

    public GetTopFailedQuestionsQueryHandler(IGenericRepository<StudentQuestionAnswer> answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<IReadOnlyList<FailedQuestionDto>> Handle(GetTopFailedQuestionsQuery request, CancellationToken cancellationToken)
    {
        var query = _answerRepository.Get(a => !a.IsDeleted && !a.Question.IsDeleted && !a.Attempt.IsDeleted && a.IsCorrect.HasValue);

        if (request.DiplomaId.HasValue)
        {
            var diplomaId = request.DiplomaId.Value;
            query = query.Where(a => a.Attempt.Quiz.DiplomaId == diplomaId);
        }

        if (request.StartDate.HasValue)
        {
            var startDate = request.StartDate.Value;
            query = query.Where(a => a.AnsweredAt >= startDate);
        }

        if (request.EndDate.HasValue)
        {
            var endDate = request.EndDate.Value;
            query = query.Where(a => a.AnsweredAt <= endDate);
        }

        var grouped = await query
            .GroupBy(a => new { a.QuestionId, a.Question.Text, QuizTitle = a.Question.Quiz.Title })
            .Select(g => new
            {
                QuestionId = g.Key.QuestionId,
                QuestionText = g.Key.Text,
                QuizTitle = g.Key.QuizTitle,
                TotalAnswers = g.Count(),
                CorrectAnswers = g.Count(a => a.IsCorrect == true)
            })
            .ToListAsync(cancellationToken);

        var result = new List<FailedQuestionDto>();

        foreach (var item in grouped)
        {
            double correctRate = 0.0;
            if (item.TotalAnswers > 0)
            {
                correctRate = Math.Round((double)item.CorrectAnswers / item.TotalAnswers * 100.0, 2);
            }

            if (correctRate < 40.0)
            {
                result.Add(new FailedQuestionDto(
                    item.QuestionId,
                    item.QuestionText,
                    item.QuizTitle,
                    item.TotalAnswers,
                    item.CorrectAnswers,
                    correctRate
                ));
            }
        }

        return result.OrderBy(q => q.CorrectRate).ToList();
    }
}
