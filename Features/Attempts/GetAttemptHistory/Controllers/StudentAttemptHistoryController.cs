using exam_system.Features.Attempts.GetAttemptHistory.DTOs;
using exam_system.Features.Attempts.GetAttemptHistory.Queries;
using exam_system.Features.Attempts.GetAttemptHistory.ViewModels;
using exam_system.Features.Diplomas.BrowseDiplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.GetAttemptHistory.Controllers
{
    [Tags(tags: "Diplomas")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttemptHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentAttemptHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

            [HttpGet]
            public async Task<IActionResult> GetStudentAttemptHistory(
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                CancellationToken cancellationToken = default)
            {
                var requestResponse = await _mediator.Send(
                    new GetStudentAttemptHistoryQuery(pageNumber, pageSize),
                    cancellationToken);

                if (!requestResponse.Success)
                    return BadRequest(requestResponse);

                var data = requestResponse.Data!;

                var attemptViewModels = data.Items
                    .Select(attempt => new StudentAttemptViewModel
                    {
                        QuizTitle = attempt.QuizTitle,
                        Status = attempt.Status,
                        Score = attempt.Score,
                        SubmittedAt = attempt.SubmittedAt,
                        QuizQuestionsCount = attempt.QuizQuestionsCount,
                        CorrectAnswerCount = attempt.CorrectAnswerCount,
                        QuizDurationMinutes = attempt.QuizDurationMinutes,
                        AttemptTakeTimeInMin = attempt.AttemptTakeTimeInMin
                    })
                    .ToList();

                var viewModel = new StudentAttemptsHistoryViewModel
                {
                    Items = attemptViewModels,
                    PageIndex = data.PageIndex,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    TotalPages = data.TotalPages,
                    HasPreviousPage = data.HasPreviousPage,
                    HasNextPage = data.HasNextPage
                };

                var endpointResponse =
                    new EndpointResponse<StudentAttemptsHistoryViewModel>
                    {
                        Success = requestResponse.Success,
                        Message = requestResponse.Message,
                        Data = viewModel
                    };

                return Ok(endpointResponse);
            }
        }
    
}
