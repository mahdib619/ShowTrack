using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class ReadShowScheduleDto
{
    public required string Id { get; init; }
    public required string ShowId { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public required string Season { get; init; }

    public static ReadShowScheduleDto FromEntity(ShowSchedule entity) => new()
    {
        Id = entity.Id,
        ShowId = entity.ShowId,
        Season = entity.Season,
        ReleaseDate = entity.ReleaseDate
    };
}