using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class UpdateShowDto
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required int CurrentSeason { get; set; }
    public required bool IsEnded { get; set; }

    public void UpdateEntity(Show show)
    {
        show.Title = Title;
        show.CurrentSeason = CurrentSeason;
        show.IsEnded = IsEnded;
    }

    public static UpdateShowDto FromReadDto(ReadShowDto readDto) => new()
    {
        Id = readDto.Id,
        Title = readDto.Title,
        CurrentSeason = readDto.CurrentSeason,
        IsEnded = readDto.IsEnded
    };
}
