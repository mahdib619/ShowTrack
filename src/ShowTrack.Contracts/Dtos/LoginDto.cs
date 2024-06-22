namespace ShowTrack.Contracts.Dtos;

public sealed class LoginDto
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? TwoFactorCode { get; init; }
    public string? TwoFactorRecoveryCode { get; init; }
}
