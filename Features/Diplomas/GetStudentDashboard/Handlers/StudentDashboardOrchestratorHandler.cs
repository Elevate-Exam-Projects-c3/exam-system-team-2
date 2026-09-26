using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Orchestrators;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.CurrentUser;
using MediatR;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers
{
    public class StudentDashboardOrchestratorHandler:IRequestHandler<StudentDashboardOrchestrator, RequestResponse<StudentDashboardDto>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserId _currentUser;
        public StudentDashboardOrchestratorHandler(IMediator mediator,ICurrentUserId currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<RequestResponse<StudentDashboardDto>> Handle(
            StudentDashboardOrchestrator request,
            CancellationToken cancellationToken)
        {

            var studentId = _currentUser.GetStudentId();
            //var studentId = Guid.Parse("AAAAAAAA-1111-1111-1111-AAAAAAAAAAAA");
            if (!studentId.HasValue)
                return RequestResponse<StudentDashboardDto>
                    .Fail("User is not authenticated.", 401);

            // 1. Retrieve basic information about the current student.
            var student = await _mediator.Send(
                new StudentDetailsQuery(studentId.Value),
                cancellationToken);

            if (!student.Success || student.Data == null)
            {
                return RequestResponse<StudentDashboardDto>
                    .Fail($"{student.Message} or can't retrieve student information.");
            }


            // 2. Retrieve only diplomas in which the current student is enrolled.
            var studentEnrollmentDiplomas = await _mediator.Send(
                new StudentEnrolledDiplomasQuery(studentId.Value),
                cancellationToken);

            if (!studentEnrollmentDiplomas.Success ||
                studentEnrollmentDiplomas.Data == null)
            {
                return RequestResponse<StudentDashboardDto>
                    .Fail("Can't retrieve student diplomas.");
            }


            // 3. Retrieve the latest submitted attempt for each quiz.
            var studentAttempts = await _mediator.Send(
                new StudentQuizAttemptsQuery(studentId.Value),
                cancellationToken);

            if (!studentAttempts.Success || studentAttempts.Data == null)
            {
                return RequestResponse<StudentDashboardDto>
                    .Fail("Can't retrieve student quiz attempts.");
            }


            var attempts = studentAttempts.Data;


            // 4. Calculate the total number of quizzes
            //    available in all enrolled diplomas.
            var totalQuizzesCount = studentEnrollmentDiplomas.Data
                .Sum(d => d.TotalQuizzesCount);


            // 5. Calculate average score based on
            //    the latest attempt of each quiz.
            var averageScore = attempts
                .Where(a => a.Score.HasValue)
                .Select(a => a.Score!.Value)
                .DefaultIfEmpty(0)
                .Average();


            // 6. Calculate pass rate based on
            //    the latest attempt of each quiz.
            var passRate = attempts.Any()
                ? (double)attempts.Count(a => a.Passed == true)
                  / attempts.Count()
                  * 100
                : 0;


            // 7. Calculate total time spent on the latest attempt
            //    of each quiz.
            var timeSpentInMinutes = attempts
                .Where(a => a.SubmittedAt > a.StartTime)
                .Sum(a => (a.SubmittedAt - a.StartTime).TotalMinutes);


            // 8. Calculate the total number of correct answers
            //    from the latest attempt of each quiz.
            var correctAnswersCount = attempts
                .Sum(a => a.CorrectAnswersCount);


            // 9. Combine all data into the final dashboard DTO.
            var studentDashboardDto = new StudentDashboardDto
            {
                Id = student.Data.Id,

                FullName = student.Data.FullName,

                TotalQuizzesCount = totalQuizzesCount,

                Statistics = new StudentDashboardStatisticsDto
                {
                    AverageScore = averageScore,

                    PassRate = passRate,

                    TimeSpentInMinutes = timeSpentInMinutes,

                    CorrectAnswersCount = correctAnswersCount
                },

                Diplomas = studentEnrollmentDiplomas.Data.ToList(),

                RecentAttempts = attempts.ToList()
            };


            return RequestResponse<StudentDashboardDto>
                .Ok(
                    studentDashboardDto,
                    student.Message);
        }
    }
}
