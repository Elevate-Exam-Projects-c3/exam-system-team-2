using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers
{
    public class AddDiplomaCommandHandler : IRequestHandler<AddDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AddDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepo, IUnitOfWork unitOfWork)
        {
            _diplomaRepo = diplomaRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Guid>> Handle(AddDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diplomaExists = await _diplomaRepo
                .GetAll()
                .AnyAsync(d => d.Title == request.Title, cancellationToken);


            if (diplomaExists)
                return RequestResponse<Guid>.Fail($"Diploma with title {request.Title} title already exists.");


            var diploma = new Diploma
            {
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl
            };

            await _diplomaRepo.AddAsync(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResponse<Guid>.Ok(diploma.Id, "Diploma added successfully.");
        }
    }
}
