namespace exam_system.Features.Attempts.GetAttemptHistory.ViewModels
{
    public class StudentAttemptsHistoryViewModel
    {
        public IReadOnlyList<StudentAttemptViewModel> Items { get; set; }
        = new List<StudentAttemptViewModel>();

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
