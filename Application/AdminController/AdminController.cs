using exam_system.Common.Enums;
using exam_system.Features.Quizzes.AdminCreateQuiz.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Application.AdminController
{
    [Controller]
    [Route("[controller]/[action]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController : BaseController
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            
            return CustomResult(result);
        }
    }
}
