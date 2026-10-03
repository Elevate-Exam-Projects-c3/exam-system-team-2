using exam_system.Features.Diplomas.GetDiplomaDetail.Orchestrators;
using exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Endpoints
{
    public static class DiplomaEndpoints
    {
        public static void MapDiplomaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/diplomas")
                .WithTags("Diplomas")
                .RequireAuthorization("Student");
            group.MapGet("/{diplomaId}", async (Guid diplomaId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var requestResponse = await mediator.Send(new GetDiplomaDetailOrchestrator(diplomaId), cancellationToken);

                if (!requestResponse.Success)
                {

                    return Results.BadRequest(requestResponse);
                }

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


                return Results.Ok(endpointResponse);
            })
            .WithName("GetDiplomaDetail")
            .WithDescription("get diploma details");
        }
    }
}
