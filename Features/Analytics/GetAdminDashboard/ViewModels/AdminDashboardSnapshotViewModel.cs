namespace exam_system.Features.Analytics.GetAdminDashboard.ViewModels;

public record AdminDashboardSnapshotViewModel(
    int TotalRegisteredUsers,
    int ActiveUsersToday,
    int TotalDiplomas,
    int TotalQuizzes,
    int TotalAttempts,
    double OverallAveragePassRate,
    DateTime SnapshotGeneratedAtUtc
);
