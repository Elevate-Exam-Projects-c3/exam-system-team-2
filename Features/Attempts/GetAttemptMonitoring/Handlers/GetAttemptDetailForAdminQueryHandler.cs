using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Attempts.GetAttemptMonitoring.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Handlers
{
    public class GetAttemptDetailForAdminQueryHandler : IRequestHandler<GetAttemptDetailForAdminQuery, RequestResponse<AttemptDetailForAdminDto>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public GetAttemptDetailForAdminQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }
        public async Task<RequestResponse<AttemptDetailForAdminDto>> Handle(GetAttemptDetailForAdminQuery request, CancellationToken cancellationToken)
        {
            //1. Validate Attempt ID

            if (request.AttemptId == Guid.Empty)
            {
                return RequestResponse<AttemptDetailForAdminDto>.Fail("AttemptId is required", statusCode: 400);
            }
            //2. Get attempt details with questions and answers
            var attempt = await _attemptRepository.Get(a => a.Id == request.AttemptId && !a.IsDeleted)
                  .Select(a => new
                  {
                      a.Id,
                      a.Score,
                      a.Passed,
                      // Get all questions for the quiz
                      Questions = a.Quiz.Questions
                         .Where(q => !q.IsDeleted)
                         .OrderBy(q => q.OrderIndex)
                         .Select(q => new
                         {
                             q.Id,
                             q.Text,
                             q.Explanation,
                             // Get the correct answer
                             CorrectOptionText = q.Options
                               .Where(o => !o.IsDeleted && o.IsCorrect)
                               .Select(o => o.OptionText)
                               .FirstOrDefault(),
                             // Get student's answer
                             Answer = a.Answers
                               .Where(ans => ans.QuestionId == q.Id && !ans.IsDeleted)
                               .Select(ans => new
                               {
                                   SelectedOptionText = ans.SelectedOption != null ? ans.SelectedOption.OptionText : null,
                                   ans.IsCorrect
                               }).FirstOrDefault()

                         }).ToList()

                  }).FirstOrDefaultAsync(cancellationToken);

            //3. Check if attempt exists
            if (attempt == null)
            {
                return RequestResponse<AttemptDetailForAdminDto>.Fail( $"Attempt with ID {request.AttemptId} was not found.",statusCode: 404);
            }

            //4. Map data to DTO
            var dto = new AttemptDetailForAdminDto
            {
                Score = attempt.Score,
                Passed = attempt.Passed,

                Questions = attempt.Questions
                   .Select(q => new AdminQuestionResultDto
                   {
                       QuestionText = q.Text,
                       Explanation = q.Explanation,
                       SelectedOptionText = q.Answer?.SelectedOptionText,
                       IsCorrect = q.Answer?.IsCorrect ?? false,
                       CorrectOptionText = q.CorrectOptionText
                   }).ToList()
            };

            return RequestResponse<AttemptDetailForAdminDto>.Ok( dto,"Attempt details retrieved successfully.");
        } 
    }
}
