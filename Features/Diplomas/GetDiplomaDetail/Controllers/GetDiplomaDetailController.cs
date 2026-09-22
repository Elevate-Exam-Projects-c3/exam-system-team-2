using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Orchestrators;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Controllers
{
    [Tags(tags: "Diplomas")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetDiplomaDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetDiplomaDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Student")]
        [HttpGet("{diplomaId}")]
        public async Task<IActionResult> GetDiplomaDetail(Guid diplomaId, CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(new GetDiplomaDetailOrchestrator(diplomaId), cancellationToken);

            if (!requestResponse.Success)
                return BadRequest(requestResponse);

            var dto = requestResponse.Data;


            var viewModel = new ViewDiplomaDetailsViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,

                Quizzes = dto.Quizzes.Select(q => new DiplomaQuizDetailsViewModel
                {
                    Quiz = new QuizDetailsViewModel
                    {
                        Id = q.Quiz.Id,
                        Title = q.Quiz.Title,
                        DurationMinutes = q.Quiz.DurationMinutes,
                        PassScore = q.Quiz.PassScore,
                        MaxAttempts = q.Quiz.MaxAttempts
                    },

                    StudentAttempt = new StudentAttemptViewModel
                    {
                        QuizId = q.StudentAttempt.QuizId,
                        AttemptCount = q.StudentAttempt.AttemptCount,
                        IsResumable = q.StudentAttempt.IsResumable,
                        CanStudentAttempt = q.StudentAttempt.CanStudentAttempt
                    }
                }).ToList()
            };


            var endpointResponse = new EndpointResponse
            {
                Success = requestResponse.Success,
                Message = requestResponse.Message,
                Data = viewModel
            };
            

            return Ok(endpointResponse);
        }
    }
}
