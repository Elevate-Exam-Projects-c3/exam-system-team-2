using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers
{
    public class GetDiplomaDetailQueryHandler:IRequestHandler<GetDiplomaDetailByIdQuery, RequestResponse<DiplomaDetailDto?>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        public GetDiplomaDetailQueryHandler(IGenericRepository<Diploma> diplomaRepo)
        {
            _diplomaRepo = diplomaRepo;
        }
        public async Task<RequestResponse<DiplomaDetailDto?>> Handle(GetDiplomaDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var exsitingDiploma = await _diplomaRepo.GetAll()
                .Where(d => d.Id == request.DiplomaId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (exsitingDiploma == null)
                return RequestResponse<DiplomaDetailDto?>
                    .Fail($"Diploma with Id = {request.DiplomaId} not found.", 404);
            
            var diplomaDto = new DiplomaDetailDto()
            {
                Id = exsitingDiploma.Id,
                Title = exsitingDiploma.Title,
                Description = exsitingDiploma.Description,
                ImageUrl = exsitingDiploma.ImageUrl,
            };

            return RequestResponse<DiplomaDetailDto?>.Ok(diplomaDto, 
                "Diploma retrieved successfully.");
        }
    }
}
