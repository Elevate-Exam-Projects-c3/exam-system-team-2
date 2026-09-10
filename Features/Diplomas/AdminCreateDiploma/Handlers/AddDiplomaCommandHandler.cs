using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers
{
    public class AddDiplomaCommandHandler : IRequestHandler<AddDiplomaCommand, RequestResponse<Unit>>
    {
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public AddDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepo, IUnitOfWork unitOfWork)
        {
            _diplomaRepo = diplomaRepo;
            _unitOfWork = unitOfWork;   
        }
        #endregion

        #region Handle Operation 
        public async Task<RequestResponse<Unit>> Handle(AddDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diplomaExists = await _diplomaRepo.GetAll().AnyAsync(d => d.Title == request.Title, cancellationToken);


            if (diplomaExists) 
                return RequestResponse<Unit>.Fail($"Diploma with title {request.Title} title already exists.");
            

            var diploma = new Diploma
            {
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl
            };

            await _diplomaRepo.AddAsync(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return RequestResponse<Unit>.Ok(Unit.Value, "Diploma added successfully.");
        }
        #endregion
    }
}
