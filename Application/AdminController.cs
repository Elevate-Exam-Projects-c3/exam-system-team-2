using exam_system.Common.Enums;
using exam_system.Controllers;
using exam_system.Features.Quizzes.AdminCreateQuiz.Mediator;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exam_system.Application
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController : BaseController
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
