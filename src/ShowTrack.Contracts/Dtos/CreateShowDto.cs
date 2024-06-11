using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class CreateShowDto
{
    public required string Title { get; set; }
    public required string UserId { get; set; }
    public required string CurrentSeason { get; set; }

    public static CreateShowDto New() => new() { UserId = "", CurrentSeason = "", Title = "" };

    public Show ToEntity() => new()
    {
        Title = Title,
        UserId = UserId,
        CurrentSeason = CurrentSeason
    };
}
