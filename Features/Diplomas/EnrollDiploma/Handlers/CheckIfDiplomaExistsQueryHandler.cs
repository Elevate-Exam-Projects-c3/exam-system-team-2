using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Diplomas.GetDiplomaForEnrollment.Dtos;
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
        #region Fields
        private readonly IGenericRepository<Diploma> _diplomaRepository;
        #endregion

        #region Constructor
        public CheckIfDiplomaExistsQueryHandler(IGenericRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }
        #endregion

        #region Handler Operations
        public async Task<RequestResponse<Unit>> Handle(CheckIfDiplomaExistsQueryQuery request, CancellationToken cancellationToken)
        {            
            var diploma = await  _diplomaRepository.GetAll()
                .Where(d => d.Id == request.DiplomaId)
                .Where(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published))
                .FirstOrDefaultAsync(cancellationToken);

            if (diploma == null)
                return RequestResponse<Unit>.Fail("Diploma not found or has been deleted.");

            return RequestResponse<Unit>.Ok(Unit.Value, "Diploma retrieved successfully.");
        }
        #endregion


    }
}
