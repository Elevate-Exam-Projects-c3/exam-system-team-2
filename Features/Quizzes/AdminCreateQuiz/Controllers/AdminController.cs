using exam_system.Common.Enums;
using exam_system.Features.Quizzes;
using exam_system.Features.Quizzes.AdminCreateQuiz.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers
{
    [Tags("Quizzes")]
    [Controller]
    [Route("[controller]/[action]")]
   // [Authorize(Roles = "Admin")]
    public class AdminCreateController : BaseController
    {
        private readonly IMediator mediator;

        public AdminCreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command)
        {
            var result = await mediator.Send(command);
            
            return CustomResult(result);
        }
    }
}
