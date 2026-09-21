using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class CheckIfDiplomaExistsQueryHandler : IRequestHandler<CheckIfDiplomaExistsQueryQuery, RequestResponse<Unit>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepository;

        public CheckIfDiplomaExistsQueryHandler(IGenericRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }

        public async Task<RequestResponse<Unit>> Handle(CheckIfDiplomaExistsQueryQuery request, CancellationToken cancellationToken)
        {
            var diplomaExists = await _diplomaRepository.GetAll()
                .Where(d => d.Id == request.DiplomaId)
                .Where(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published))
                .AnyAsync(cancellationToken);

            if (!diplomaExists)
                return RequestResponse<Unit>.Fail("Diploma not found or has no published quizzes.", 404);

            return RequestResponse<Unit>.Ok(Unit.Value, "Diploma exists.");
        }


    }
}
