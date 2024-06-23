using System.Security.Claims;

namespace ShowTrack.Client.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated is true ? principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value : null;
    }
}
