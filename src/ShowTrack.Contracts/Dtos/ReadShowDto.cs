using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public class ReadShowDto
{
    public required string Id { get; init; }
    public required string Title { get; set; }
    public required string UserId { get; set; }
    public int CurrentSeason { get; set; }
    public ReadShowScheduleDto? Schedule { get; set; }
    public bool IsEnded { get; set; }
    public int PersonalRating { get; set; } = -1;

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