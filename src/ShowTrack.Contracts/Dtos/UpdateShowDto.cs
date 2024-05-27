using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public class UpdateShowDto
{
    public required string Id { get; set; }
    public required string Title { get; init; }
    public required string CurrentSeason { get; init; }

    public void UpdateEntity(Show show)
    {
        show.Title = Title;
        show.CurrentSeason = CurrentSeason;
    }
}
