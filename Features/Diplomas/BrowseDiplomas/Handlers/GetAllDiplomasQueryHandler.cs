using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers
{
    public class GetAllDiplomasQueryHandler:IRequestHandler<GetAllDiplomasQuery,RequestResponse<PaginatedResult<DiplomaDto>>>
    {
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        #endregion

        #region Constructor
        public GetAllDiplomasQueryHandler(IGenericRepository<Diploma> diplomaRepo)
        {
            _diplomaRepo = diplomaRepo;
        }
        #endregion

        #region Handle Method
        public async Task<RequestResponse<PaginatedResult<DiplomaDto>>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;

            var totalCount = await _diplomaRepo.GetAll()
                .CountAsync(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published),
                cancellationToken);

            var diplomas = _diplomaRepo.GetAll()
                .Where(d => d.Quizzes.Any(q=>q.Status==QuizStatus.Published))
                .AsNoTracking()
                .Skip(skip)
                .Take(request.PageSize);
            
            
            var diplomaDtos = await diplomas
                .Select(d => new DiplomaDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    CountOfQuizzes = d.Quizzes.Count(q => q.Status == QuizStatus.Published)
                }).ToListAsync(cancellationToken);
            //untill now not handle student's own progress => 2/5
            
            var paginatedResult = PaginatedResult<DiplomaDto>.Create(
                diplomaDtos, 
                totalCount, 
                request.PageNumber, 
                request.PageSize
            );

            return RequestResponse<PaginatedResult<DiplomaDto>>.Ok(paginatedResult, "Diplomas retrieved successfully.");
        }
        #endregion
    }
}
