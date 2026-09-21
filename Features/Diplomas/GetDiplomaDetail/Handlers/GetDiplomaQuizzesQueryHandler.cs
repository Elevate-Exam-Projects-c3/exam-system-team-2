using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Pkcs;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers
{
    public class GetDiplomaQuizzesQueryHandler : IRequestHandler<GetDiplomaQuizzesQuery, RequestResponse<IEnumerable<QuizDetailsDto>>>
    {

        private readonly IGenericRepository<Quiz> _quizRepo;
        public GetDiplomaQuizzesQueryHandler(IGenericRepository<Quiz> quizRepo)
        {
            _quizRepo = quizRepo;
        }

        public async Task<RequestResponse<IEnumerable<QuizDetailsDto>>> Handle(GetDiplomaQuizzesQuery request, CancellationToken cancellationToken)
        {

            // 1. Get published quizzes for this diploma
            var quizzes = await _quizRepo.GetAll()
                .Where(
                    q => q.DiplomaId == request.DiplomaId &&
                    q.Status == QuizStatus.Published)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!quizzes.Any())
                return RequestResponse<IEnumerable<QuizDetailsDto>>
                    .Fail($"No published quizzes found for Diploma with Id = {request.DiplomaId}.", 404);

            //Build Dtos
            var quizDtos = quizzes.Select(q => new QuizDetailsDto
            {
                Id = q.Id,
                Title = q.Title,
                DurationMinutes = q.DurationMinutes,
                PassScore = q.PassScore,
                MaxAttempts = q.MaxAttempts
            }).ToList();

            return RequestResponse<IEnumerable<QuizDetailsDto>>
                .Ok(quizDtos, "Published quizzes retrieved successfully.");

        }
    }
}
