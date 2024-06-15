using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class UpdateShowDto
{
    public required string Id { get; set; }
    public required string Title { get; init; }
    public required string CurrentSeason { get; init; }
    public required bool IsEnded { get; set; }

    public void UpdateEntity(Show show)
    {
        show.Title = Title;
        show.CurrentSeason = CurrentSeason;
        show.IsEnded = IsEnded;
    }
}
