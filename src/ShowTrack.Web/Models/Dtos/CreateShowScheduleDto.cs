using ShowTrack.Domain.Entities;

namespace ShowTrack.Web.Models.Dtos;

public sealed class CreateShowScheduleDto
{
    public required string ShowId { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public required string Season { get; init; }

    public ShowSchedule ToEntity() => new()
    {
        ShowId = ShowId,
        ReleaseDate = ReleaseDate,
        Season = Season
    };
}
