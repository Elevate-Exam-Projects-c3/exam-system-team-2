using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers
{
    public class GetDiplomaDetailCommandHandler:IRequestHandler<GetDiplomaDetailByIdQuery, RequestResponse<DiplomaDto?>>
    {
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        #endregion
        #region Constructor 
        public GetDiplomaDetailCommandHandler(IGenericRepository<Diploma> diplomaRepo)
        {
            _diplomaRepo = diplomaRepo;
        }
        #endregion
        #region Handle Method
        public async Task<RequestResponse<DiplomaDto?>> Handle(GetDiplomaDetailByIdQuery request, CancellationToken cancellationToken)
        { 
            var exsitingDiploma = await _diplomaRepo.GetByIdAsync(request.Id);
   
            if(exsitingDiploma == null)
                return RequestResponse<DiplomaDto?>.Fail(
                    $"Diploma with Id = {request.Id} not found.");
            

            DiplomaDto diplomaDto = new DiplomaDto()
            {
                //Id = exsitingDiploma.Id,
                Description = exsitingDiploma.Description,
                ImageUrl = exsitingDiploma.ImageUrl,
                //CreatedAt = exsitingDiploma.CreatedAt,
                //UpdatedAt = exsitingDiploma.UpdatedAt,
                //IsDeleted = exsitingDiploma.IsDeleted,
                //DeletedAt = exsitingDiploma.DeletedAt
            };

            return RequestResponse<DiplomaDto?>.Ok(diplomaDto, 
                "Diploma retrieved successfully.");
        }
        #endregion
    }
}
