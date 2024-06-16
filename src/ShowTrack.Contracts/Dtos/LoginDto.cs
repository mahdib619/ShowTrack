namespace ShowTrack.Contracts.Dtos;

public sealed class LoginDto
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? TwoFactorCode { get; init; }
    public string? TwoFactorRecoveryCode { get; init; }
}
