using ShowTrack.Domain.Entities;

namespace ShowTrack.Contracts.Dtos;

public sealed class UpdateShowDto
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public int CurrentSeason { get; set; }
    public bool IsEnded { get; set; }
    public int? PersonalRating { get; set; }
    public DateTime DatePinned { get; set; }

    public void UpdateEntity(Show show)
    {
        show.Title = Title;
        show.CurrentSeason = CurrentSeason;
        show.IsEnded = IsEnded;
        show.PersonalRating = PersonalRating;
        show.DatePinned = DatePinned;
    }

    public static UpdateShowDto FromReadDto(ReadShowDto readDto) => new()
    {
        Id = readDto.Id,
        Title = readDto.Title,
        CurrentSeason = readDto.CurrentSeason,
        IsEnded = readDto.IsEnded,
        PersonalRating = readDto.PersonalRating < 0 ? null : readDto.PersonalRating,
        DatePinned = readDto.DatePinned
    };
}
