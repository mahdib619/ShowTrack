namespace ShowTrack.Web.Models;

public class ClientError
{
    public required string Message { get; set; }
    public required int StatusCode { get; set; }
    public object? Details { get; set; }
}
