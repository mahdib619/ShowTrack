namespace ShowTrack.Contracts.Dtos;

public sealed class UserInfo
{
    public bool IsAuthenticated { get; init; }
    public string? UserName { get; set; }
    public Dictionary<string, string>? Claims { get; init; }
}
