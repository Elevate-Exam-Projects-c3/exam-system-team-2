using exam_system.Common.Enums;
<<<<<<< HEAD:Features/Quizzes/AdminCreateQuiz/Controllers/AdminController.cs
using exam_system.Features.Quizzes;
=======
using exam_system.Controllers;
>>>>>>> master:Application/AdminController.cs
using exam_system.Features.Quizzes.AdminCreateQuiz.Mediator;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
<<<<<<< HEAD:Features/Quizzes/AdminCreateQuiz/Controllers/AdminController.cs
    public class AdminCreateController : BaseController
=======
    public class AdminController : BaseController
>>>>>>> master:Application/AdminController.cs
    {
        private readonly IMediator mediator;

        public AdminCreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

<<<<<<< HEAD:Features/Quizzes/AdminCreateQuiz/Controllers/AdminController.cs
        [HttpPost("create")]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command)
        {
            var result = await mediator.Send(command);
            
            return CustomResult(result);
=======
        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return HandleResult(result);
>>>>>>> master:Application/AdminController.cs
        }
    }
}
