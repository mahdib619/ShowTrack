using Microsoft.AspNetCore.Identity;

namespace ShowTrack.Domain.Entities;

public sealed class Show
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string Title { get; set; }

    public required string UserId { get; init; }
    public IdentityUser? User { get; init; }

    public int CurrentSeason { get; set; }

    public ShowSchedule? Schedule { get; set; }

    public bool IsEnded { get; set; }

    public DateTime DateAdded { get; init; } = DateTime.Now;

    public int? PersonalRating { get; set; }

    //public DateTime? DatePinned { get; set; }
}