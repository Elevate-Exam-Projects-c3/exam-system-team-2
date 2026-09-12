namespace exam_system.Features.Shared
{
    public static class RequestResponseExtensions
    {
        public static RequestResponse<TOut> MapTo<TIn, TOut>(this RequestResponse<TIn> source, Func<TIn, TOut> map)
        {
            return new RequestResponse<TOut>
            {
                Success = source.Success,
                StatusCode = source.StatusCode,
                Message = source.Message,
                Data = source.Data is null ? default : map(source.Data),
                Errors = source.Errors
            };
        }
    }
}
