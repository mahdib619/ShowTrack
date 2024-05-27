using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class UpdateShowScheduleDto
{
    public required string ShowId { get; set; }
    public DateOnly ReleaseDate { get; init; }
    public required string Season { get; init; }

    public ShowSchedule ToEntity() => new()
    {
        ShowId = ShowId,
        ReleaseDate = ReleaseDate,
        Season = Season
    };

    public void UpdateEntity(ShowSchedule schedule)
    {
        schedule.ReleaseDate = ReleaseDate;
        schedule.Season = Season;
    }
}
