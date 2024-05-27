using Microsoft.AspNetCore.Identity;

namespace ShowTrack.Domain.Entities;

public sealed class Show
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string Title { get; set; }

    public required string UserId { get; init; }
    public IdentityUser? User { get; init; }

    public required string CurrentSeason { get; set; }

    public ShowSchedule? Schedule { get; set; }
}