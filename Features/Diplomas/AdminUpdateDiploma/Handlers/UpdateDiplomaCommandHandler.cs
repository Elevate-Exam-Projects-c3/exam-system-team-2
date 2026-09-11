using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.ViewModels;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Handlers
{
    public class UpdateDiplomaCommandHandler:IRequestHandler<UpdateDiplomaCommand, RequestResponse<Unit>>
    {
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public UpdateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepo, IUnitOfWork unitOfWork)
        {
            _diplomaRepo = diplomaRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Handle Operations
       public async Task<RequestResponse<Unit>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var existingDiploma = await _diplomaRepo.GetByIdAsync(request.Id);
            if (existingDiploma == null)
                return RequestResponse<Unit>.Fail($"Diploma with id {request.Id} not found.");


            if (!string.IsNullOrEmpty(request.Title))
            {
                var titleExists = await _diplomaRepo
                    .GetAll()
                    .AnyAsync(d => d.Title == request.Title && d.Id != request.Id, cancellationToken);

                if(titleExists)
                    return RequestResponse<Unit>.Fail($"Diploma with title '{request.Title}' already exists.");

                existingDiploma.Title = request.Title;
            }


            if (request.Description != null)
                existingDiploma.Description = request.Description;
            

            if (!string.IsNullOrEmpty(request.ImageUrl))
                existingDiploma.ImageUrl = request.ImageUrl;


            _diplomaRepo.Update(existingDiploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse<Unit>.Ok(Unit.Value,"Diploma is updated succussfly."); 
        }
        #endregion
    }
}
