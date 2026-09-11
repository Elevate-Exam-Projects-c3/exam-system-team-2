namespace exam_system.Features.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorMessage)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new InvalidOperationException("A successful result cannot have an error message.");
            }

            if (!isSuccess && string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new InvalidOperationException("A failing result must have an error message.");
            }

            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, string.Empty);

        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

    public class Result<T> : Result
    {
        private readonly T? _data;

        public T Data => IsSuccess
            ? _data!
            : throw new InvalidOperationException("Cannot access data of a failed result.");

        private Result(bool isSuccess, T? data, string errorMessage)
            : base(isSuccess, errorMessage)
        {
            _data = data;
        }

        public static Result<T> Success(T data) => new Result<T>(true, data, string.Empty);

        public static new Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
    }
}
