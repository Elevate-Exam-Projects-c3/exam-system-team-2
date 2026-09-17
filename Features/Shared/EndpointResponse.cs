namespace exam_system.Features.Shared;

public class EndpointResponse<T> : ApiResponse<T>
{
    public EndpointResponse() { }

    public EndpointResponse(bool success, int statusCode, string message, T? data = default, IDictionary<string, string[]>? errors = null)
        : base(success, statusCode, message, data, errors)
    {
    }

    public static EndpointResponse<T> FromResult(RequestResponse<T> result)
    {
        return new EndpointResponse<T>(
            result.Success,
            result.StatusCode,
            result.Message,
            result.Data,
            result.Errors
        );
    }

    public static new EndpointResponse<T> Fail(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null)
        => new(false, statusCode, message, default, errors);

    public static new EndpointResponse<T> Ok(T data, string message = "Success", int statusCode = 200)
        => new(true, statusCode, message, data);
}

public class EndpointResponse : ApiResponse
{
    public static EndpointResponse FromResult(RequestResponse result)
    {
        return new EndpointResponse
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow
        };
    }
}
