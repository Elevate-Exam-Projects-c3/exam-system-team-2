using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, RequestResponse<Unit>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepo, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _diplomaRepo = diplomaRepo;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Unit>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _diplomaRepo
                .GetAll()
                .FirstOrDefaultAsync(
                    d => d.Id == request.Id,
                    cancellationToken);

            if (diploma == null)
                return RequestResponse<Unit>.Fail("Diploma not found.", 404);

            var hasEnrollments = await _mediator.Send(
                new CheckDiplomaHasEnrollmentsQuery(request.Id), cancellationToken);

            if (hasEnrollments.Success)
                return RequestResponse<Unit>.Fail("Cannot delete diploma with existing enrollments.", 409);

            diploma.IsDeleted = true;
            diploma.DeletedAt = DateTime.UtcNow;

            await _diplomaRepo.UpdateAsync(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return RequestResponse<Unit>.Ok(Unit.Value, "Diploma deleted successfully.");


        }
    }
}
