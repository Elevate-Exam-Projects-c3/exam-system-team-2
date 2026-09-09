using exam_system.Common.Enums;
using exam_system.Features.Quizzes.AdminCreateQuiz.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exam_system.Application
{
    [Controller]
    [Route("[controller]/[action]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command)
        {
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Quiz added successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to add quiz." });
            }
        })
    }
}
