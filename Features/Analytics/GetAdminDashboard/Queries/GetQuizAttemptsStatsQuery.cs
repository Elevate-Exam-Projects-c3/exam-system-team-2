using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Queries;

public record QuizAttemptsStatsDto(int TotalAttempts, double PassRate);

public record GetQuizAttemptsStatsQuery : IRequest<QuizAttemptsStatsDto>;
