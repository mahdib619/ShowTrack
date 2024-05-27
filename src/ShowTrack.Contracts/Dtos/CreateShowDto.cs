using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class CreateShowDto
{
    public required string Title { get; init; }
    public required string UserId { get; set; }
    public required string CurrentSeason { get; init; }

    public Show ToEntity() => new()
    {
        Title = Title,
        UserId = UserId,
        CurrentSeason = CurrentSeason
    };
}
