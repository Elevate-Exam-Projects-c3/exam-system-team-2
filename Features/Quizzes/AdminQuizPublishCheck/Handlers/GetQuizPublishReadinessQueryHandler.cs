using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Dtos;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Handlers
{
    public class GetQuizPublishReadinessQueryHandler : IRequestHandler<GetQuizPublishReadinessQuery, RequestResponse<QuizPublishReadinessDto>>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;

        public GetQuizPublishReadinessQueryHandler(IGenericRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }
        public async Task<RequestResponse<QuizPublishReadinessDto>> Handle(GetQuizPublishReadinessQuery request, CancellationToken cancellationToken)
        {
            //1. Get Quiz With Questions and Question Options :
            var quiz = await _quizRepository.Get(q => q.Id == request.QuizId)
            .Select(q => new{q.Id,q.DurationMinutes,q.PassScore, Questions = q.Questions
            .Where(question => !question.IsDeleted)
            .Select(question => new
            {
                CorrectOptionsCount = question.Options.Count(option =>!option.IsDeleted &&option.IsCorrect)
            }).ToList() }) .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return RequestResponse<QuizPublishReadinessDto>.Fail($"Quiz with ID {request.QuizId} was not found.", 404);
            }

            //2.Build The Checks List :
            var checks = new List<PublishCheckItemDto>();

            //Check 1: Quiz Must Have at Least One Question :

            var HasQuestions = quiz.Questions.Any();
            checks.Add(new PublishCheckItemDto
            {
                CheckName = "Quiz Must Have at Least One Question",
                Passed = HasQuestions,
                Message = HasQuestions ? null : "Quiz must have at least one question to be published."
            });

            //Check 2: Every question has exactly one correct option :
            var allQuestionsValid = true;
            foreach (var question in quiz.Questions)
            {
                if (question.CorrectOptionsCount != 1)
                {
                    allQuestionsValid = false;
                    break;
                }

            }
            checks.Add(new PublishCheckItemDto
            {
                CheckName = "All Questions Have Exactly One Correct Option",
                Passed = allQuestionsValid,
                Message = allQuestionsValid ? null : "Every question must have exactly one correct option."
            });

            // Check 3: DurationMinutes is valid :
            var validDuration = quiz.DurationMinutes > 0;
            checks.Add(new PublishCheckItemDto
            {
                CheckName = "Duration Minutes IsValid",
                Passed = validDuration,
                Message = validDuration ? null : "DurationMinutes must be greater than 0."
            });

            // Check 4: PassScore is within 0-100
            var validPassScore = quiz.PassScore is >= 0 and <= 100;
            checks.Add(new PublishCheckItemDto
            {
                CheckName = "Pass Score Is In Range",
                Passed = validPassScore,
                Message = validPassScore ? null : "PassScore must be between 0 and 100."
            });

            //3. Build the response Dto :
            var dto = new QuizPublishReadinessDto
            {
                QuizId = quiz.Id,
                IsReadyToPublish = checks.All(c => c.Passed),
                Checks = checks
            };

            //4. Return wrapped in RequestResponse, matching the project's Controller pattern :
            return RequestResponse<QuizPublishReadinessDto>.Ok(dto, "Publish readiness check completed.");

        }
    }
}
