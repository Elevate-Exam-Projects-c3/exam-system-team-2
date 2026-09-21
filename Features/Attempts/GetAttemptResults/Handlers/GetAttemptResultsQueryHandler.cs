using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Attempts.GetAttemptResults.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptResults.Handlers
{
    public class GetAttemptResultsQueryHandler
    : IRequestHandler<GetAttemptResultsQuery, RequestResponse<AttemptResultsDto>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public GetAttemptResultsQueryHandler(
            IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }

        public async Task<RequestResponse<AttemptResultsDto>> Handle(GetAttemptResultsQuery request, CancellationToken cancellationToken)
        {
            //1. Validate the request :
            if (request.AttemptId == Guid.Empty)
            {
                return RequestResponse<AttemptResultsDto>.Fail("AttemptId is required", statusCode: 400);
            }
            // 2. Get the Attempt and all data needed for the results
            var attempt = await _attemptRepository
                .Get(a => a.Id == request.AttemptId && !a.IsDeleted)
                .Select(a => new
                {
                    a.Id,
                    StudentUserId = a.Student.UserId,
                    a.Status,
                    a.Score,
                    a.Passed,
                    // Get the questions and answers for the attempt
                    Questions = a.Quiz.Questions
                        .Where(q => !q.IsDeleted)
                        .OrderBy(q => q.OrderIndex)
                        .Select(q => new
                        {
                            q.Id,
                            q.Text,
                            q.Explanation,
                            CorrectOptionText = q.Options
                                .Where(o => !o.IsDeleted && o.IsCorrect)
                                .Select(o => o.OptionText)
                                .FirstOrDefault(),
                            // Get the answer for the question in the attempt
                            Answer = a.Answers
                                .Where(ans => ans.QuestionId == q.Id && !ans.IsDeleted)
                                .Select(ans => new
                                {
                                    SelectedOptionText = ans.SelectedOption != null ? ans.SelectedOption.OptionText : null,
                                    ans.IsCorrect
                                }).FirstOrDefault()
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            //3. Check if the attempt exists :
            if (attempt == null)
            {
                return RequestResponse<AttemptResultsDto>.Fail($"Attempt with ID {request.AttemptId} was not found.", statusCode: 404);
            }

            //4.Ownership Check : if the caller is the owner of the attempt :
            if (attempt.StudentUserId != request.CallerUserId)
            {
                return RequestResponse<AttemptResultsDto>.Fail($"Attempt with ID {request.AttemptId} was not found.", statusCode: 404);
            }

            //5. check if the attempt Status is InProgress :
            if (attempt.Status == AttemptStatus.InProgress)
            {
                return RequestResponse<AttemptResultsDto>.Fail("Attempt is still in progress", statusCode: 400);
            }

            // 6. Map questions to the  DTO : 
            var questionResults = attempt.Questions
                .Select(q => new QuestionResultDto
                {
                    QuestionText = q.Text,
                    Explanation = q.Explanation,
                    SelectedOptionText = q.Answer?.SelectedOptionText,
                    IsCorrect = q.Answer?.IsCorrect ?? false,
                    CorrectOptionText = q.CorrectOptionText
                }).ToList();

            // 7. Create the final DTO :
            var dto = new AttemptResultsDto
            {
                Score = attempt.Score,
                Passed = attempt.Passed,
                Questions = questionResults
            };
            // 8. Return the results :
            return RequestResponse<AttemptResultsDto>.Ok(dto, "Attempt results retrieved successfully.");

        }


    }

}

