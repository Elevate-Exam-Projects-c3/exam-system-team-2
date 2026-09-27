using exam_system.Features.Attempts.GetAttemptHistory.DTOs;
using exam_system.Features.Attempts.GetAttemptHistory.Queries;
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
        public async Task<IActionResult> GetStudentAttemptHistory(int pageNumber =1 ,int pageSize = 10,CancellationToken cancellationToken=default)
        {
            var requestResponse = await _mediator.Send(
                new GetStudentAttemptHistoryQuery(pageNumber, pageSize),cancellationToken);

            var endpointResponse =
                EndpointResponse<PaginatedResult<StudentAttemptDto>>.FromResult(
                    requestResponse);

            if (!endpointResponse.Success)
            {
                return StatusCode(
                    endpointResponse.StatusCode,endpointResponse);
            }

            return Ok(endpointResponse);
        }
    }
}
