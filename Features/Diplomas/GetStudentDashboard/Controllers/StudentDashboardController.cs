using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Diplomas.GetStudentDashboard.Orchestrators;
using exam_system.Features.Diplomas.GetStudentDashboard.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Controllers
{
    [Tags(tags: "Diplomas")]
    [Authorize(Roles = "Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudentDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GEtStudentDashboard(CancellationToken cancellationToken = default)
        {
            var requestResponse = await _mediator.Send(new StudentDashboardOrchestrator(), cancellationToken);
            if (!requestResponse.Success)
                return BadRequest(requestResponse);

            var data = requestResponse.Data!;
            var viewModel = new StudentDashboardViewModel
            {
                Id = data.Id,
                FullName = data.FullName,
                TotalQuizzesCount = data.TotalQuizzesCount,

                Statistics = new StudentDashboardStatisticsViewModel
                {
                    AverageScore = data.Statistics.AverageScore,
                    PassRate = data.Statistics.PassRate,
                    TimeSpentInMinutes = data.Statistics.TimeSpentInMinutes,
                    CorrectAnswersCount = data.Statistics.CorrectAnswersCount
                },

                Diplomas = data.Diplomas
            .Select(diploma => new DiplomaStudentDashboardViewModel
            {
                Id = diploma.Id,
                Title = diploma.Title,
                Description = diploma.Description,
                ImageUrl = diploma.ImageUrl,
                TotalQuizzesCount = diploma.TotalQuizzesCount,
                TakenQuizzesCount = diploma.TakenQuizzesCount
            })
            .ToList(),

                RecentAttempts = data.RecentAttempts
            .Select(attempt => new RecentQuizAttemptViewModel
            {
                QuizId = attempt.QuizId,
                QuizTitle = attempt.QuizTitle,
                Score = attempt.Score,
                Passed = attempt.Passed,
                StartTime = attempt.StartTime,
                SubmittedAt = attempt.SubmittedAt,
                CorrectAnswersCount = attempt.CorrectAnswersCount
            })
            .ToList()
            };

            var endpointResponse =
                new EndpointResponse<StudentDashboardViewModel>
                {
                    Success = requestResponse.Success,
                    StatusCode = requestResponse.StatusCode,
                    Message = requestResponse.Message,
                    Data = viewModel,
                    Errors = requestResponse.Errors
                };

            return StatusCode(
                endpointResponse.StatusCode, endpointResponse);
        }
    }
}
