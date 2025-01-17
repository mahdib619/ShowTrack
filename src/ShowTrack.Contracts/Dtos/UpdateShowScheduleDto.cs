﻿using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class UpdateShowScheduleDto
{
    public required string ShowId { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required int? Season { get; set; }

    public ShowSchedule ToEntity() => new()
    {
        ShowId = ShowId,
        ReleaseDate = ReleaseDate,
        Season = Season ?? 0
    };

    public void UpdateEntity(ShowSchedule schedule)
    {
        schedule.ReleaseDate = ReleaseDate;
        schedule.Season = Season ?? 0;
    }
}
