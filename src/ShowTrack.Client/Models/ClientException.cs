namespace ShowTrack.Client.Models;

public class ClientException(string message) : ApplicationException(message)
{
    public static ClientException UnknownError { get; } = new("Unknown Error!");
}
