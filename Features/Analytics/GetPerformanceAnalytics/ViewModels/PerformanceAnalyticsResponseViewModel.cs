namespace exam_system.Features.Analytics.GetPerformanceAnalytics.ViewModels;

public record QuizPassRateDto(
    Guid QuizId,
    string QuizTitle,
    int PassedAttempts,
    int TotalAttempts,
    double PassRate
);

public record DiplomaAverageScoreDto(
    Guid DiplomaId,
    string DiplomaTitle,
    double AverageScore,
    int TotalAttempts
);

public record AttemptsOverTimeDto(
    string Date,
    int AttemptCount
);

public record FailedQuestionDto(
    Guid QuestionId,
    string QuestionText,
    string QuizTitle,
    int TotalAnswers,
    int CorrectAnswers,
    double CorrectRate
);

public record PerformanceAnalyticsResponseViewModel(
    IReadOnlyList<QuizPassRateDto> QuizPassRates,
    IReadOnlyList<DiplomaAverageScoreDto> DiplomaAverageScores,
    IReadOnlyList<AttemptsOverTimeDto> AttemptsOverTime,
    IReadOnlyList<FailedQuestionDto> TopFailedQuestions,
    DateTime GeneratedAtUtc
);
