using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers
{
    public class CheckDiplomaExistingByNameQueryHandler : IRequestHandler<CheckDiplomaExistingByNameQuery, RequestResponse<Unit>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        public CheckDiplomaExistingByNameQueryHandler(IGenericRepository<Diploma> diplomaRepo)
        {
            _diplomaRepo = diplomaRepo;
        }
        public async Task<RequestResponse<Unit>> Handle(CheckDiplomaExistingByNameQuery request, CancellationToken cancellationToken)
        {
            var diplomaExisted = await _diplomaRepo.GetAll()
                .AnyAsync(d => d.Title == request.Title, cancellationToken);

            if (!diplomaExisted)
                return RequestResponse<Unit>.Fail($"Can't find diploma with title {request.Title}");

            return RequestResponse<Unit>.Ok(Unit.Value, $"There is a diploma with title {request.Title}.");
        }
    }
}
