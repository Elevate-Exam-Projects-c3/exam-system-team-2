using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand,RequestResponse<Unit>>
    {
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public DeleteDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepo, IUnitOfWork unitOfWork)
        {
            _diplomaRepo = diplomaRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Handler Operations
        public async Task<RequestResponse<Unit>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _diplomaRepo        
                .GetAll()
                .Include(d => d.Enrollments)
                .FirstOrDefaultAsync(
                    d => d.Id == request.Id,
                    cancellationToken);

            if (diploma == null)
                return RequestResponse<Unit>.Fail("Diploma not found.", 404);

            if(diploma.Enrollments.Any())
                return RequestResponse<Unit>.Fail("Cannot delete diploma with existing enrollments.", 409); 
            
            diploma.IsDeleted = true;
            diploma.DeletedAt = DateTime.UtcNow;

            await _diplomaRepo.UpdateAsync(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return RequestResponse<Unit>.Ok(Unit.Value,"Diploma deleted successfully.");


        }
        #endregion
    }
}
