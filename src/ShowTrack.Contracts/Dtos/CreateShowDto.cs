using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class CreateShowDto
{
    public required string Title { get; set; }
    public required string UserId { get; set; }
    public required int? CurrentSeason { get; set; }

    public static CreateShowDto New() => new() { UserId = string.Empty, CurrentSeason = null, Title = string.Empty };

    public Show ToEntity() => new()
    {
        Title = Title,
        UserId = UserId,
        CurrentSeason = CurrentSeason ?? 0
    };
}
