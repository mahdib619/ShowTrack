namespace ShowTrack.Web.Models;

public class EmailClient
{
    public required string Host { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}
