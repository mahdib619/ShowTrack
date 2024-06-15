namespace ShowTrack.Web.Models;

public class ApiError
{
    public required string Message { get; set; }
    public required int StatusCode { get; init; }
    public object? Details { get; init; }
}

public sealed class ClientError : ApiError;