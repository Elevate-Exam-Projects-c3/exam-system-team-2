using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptMonitoring.Dtos;
using exam_system.Features.Attempts.GetAttemptMonitoring.Queries;
using Microsoft.EntityFrameworkCore;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptMonitoring.Handlers
{
    public class GetAttemptsQueryHandler : IRequestHandler<GetAttemptsQuery, RequestResponse<PaginatedResult<AttemptSummaryDto>>>
    {
        private readonly IGenericRepository<QuizAttempt> _attemptRepository;

        public GetAttemptsQueryHandler(
            IGenericRepository<QuizAttempt> attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }
        public async Task<RequestResponse<PaginatedResult<AttemptSummaryDto>>> Handle(GetAttemptsQuery request, CancellationToken cancellationToken)
        {
            //1. Validation of Pagination :
            if (request.PageIndex < 1)
            {
                return RequestResponse<PaginatedResult<AttemptSummaryDto>>.Fail("PageIndex must be greater than or equal to 1", statusCode: 400);
            }

            if (request.PageSize <= 0)
            {
                return RequestResponse<PaginatedResult<AttemptSummaryDto>>.Fail("PageSize must be greater than 0", statusCode: 400);
            }

            var query = _attemptRepository.Get(a => !a.IsDeleted);

            //2. Filtering :
            if (request.QuizId.HasValue)
            {
                query = query.Where(a => a.QuizId == request.QuizId.Value);
            }

            if (request.StudentId.HasValue)
            {
                query = query.Where(a => a.StudentId == request.StudentId.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status.Value);
            }

            //3. Sorting :
            if (request.SortOrder == AttemptSortOrder.Ascending)
            {
                query = query.OrderBy(a => a.SubmittedAt);
            }
            else
            {
                query = query.OrderByDescending(a => a.SubmittedAt);
            }

            //4. Pagination :

            // Calculate the total count before applying pagination :
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination :
            var attempts = await query
                  .Skip((request.PageIndex - 1) * request.PageSize)
                  .Take(request.PageSize)
                  .Select(a => new AttemptSummaryDto
                  {
                      AttemptId = a.Id,
                      StudentName = a.Student.User.FullName,
                      QuizTitle = a.Quiz.Title,
                      Status = a.Status,
                      Score = a.Score,
                      Passed = a.Passed,
                      SubmittedAt = a.SubmittedAt
                  }).ToListAsync(cancellationToken);

            // 5. Create PaginatedResult :
            var paginatedResult = PaginatedResult<AttemptSummaryDto>.Create(attempts, totalCount, request.PageIndex, request.PageSize);

            return RequestResponse<PaginatedResult<AttemptSummaryDto>>.Ok(paginatedResult, "Attempts retrieved successfully.");

        }
    }
}
