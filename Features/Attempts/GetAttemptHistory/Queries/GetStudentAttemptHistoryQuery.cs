using exam_system.Features.Attempts.GetAttemptHistory.DTOs;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptHistory.Queries
{
    public class GetStudentAttemptHistoryQuery(int PageNumber, int PageSize) : IRequest<RequestResponse<PaginatedResult<StudentAttemptDto>>>
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; } = PageNumber < 1 ? 1 : PageNumber;
        public int PageSize { get; } = PageSize < 1 || PageSize > MaxPageSize ? 10 : PageSize;
    }
}
