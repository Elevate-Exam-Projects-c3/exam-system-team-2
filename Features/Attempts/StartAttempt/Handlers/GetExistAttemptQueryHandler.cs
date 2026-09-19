using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Dtos;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers
{
    public class GetExistStudentAttemptQueryHandler : IRequestHandler<GetExistStudentAttemptQuery, RequestResponse<AttemptStatusDto>>
    {
        private readonly IGenericRepository<QuizAttempt> _quizAttemptRepository;

        public GetExistStudentAttemptQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepository)
        {
            _quizAttemptRepository = quizAttemptRepository;
        }
        public async Task<RequestResponse<AttemptStatusDto>> Handle(GetExistStudentAttemptQuery request, CancellationToken cancellationToken)
        {
            var attempt = _quizAttemptRepository.Get(a => a.StudentId == request.StudentId && a.QuizId == request.QuizId).
                                                 Select(a => new AttemptStatusDto
                                                 {
                                                     AttemptId = a.Id,
                                                     StudentId = a.StudentId,
                                                     Status = a.Status
                                                 }).FirstOrDefault();

            if(attempt is not null)
            {
                return RequestResponse<AttemptStatusDto>.Ok(attempt, "Existing attempt found");
            }

            return RequestResponse<AttemptStatusDto>.Fail("No existing attempt found");
        }
    }
}