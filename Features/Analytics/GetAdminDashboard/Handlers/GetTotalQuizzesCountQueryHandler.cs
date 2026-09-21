using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Analytics.GetAdminDashboard.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Handlers;

public class GetTotalQuizzesCountQueryHandler : IRequestHandler<GetTotalQuizzesCountQuery, int>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public GetTotalQuizzesCountQueryHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<int> Handle(GetTotalQuizzesCountQuery request, CancellationToken cancellationToken)
    {
        return await _quizRepository.CountAsync(q => !q.IsDeleted);
    }
}
