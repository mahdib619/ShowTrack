using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class ReadShowDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string UserId { get; init; }
    public int CurrentSeason { get; init; }
    public ReadShowScheduleDto? Schedule { get; set; }
    public bool IsEnded { get; init; }

    public static ReadShowDto FromEntity(Show entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        UserId = entity.UserId,
        CurrentSeason = entity.CurrentSeason,
        Schedule = entity.Schedule is null ? null : ReadShowScheduleDto.FromEntity(entity.Schedule),
        IsEnded = entity.IsEnded
    };
}