using ShowTrack.Client.Models.Dtos;
using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Services;

public interface IShowsService
{
    Task<PagedResponseDto<ReadShowUiDto>?> GetAllShows(PagedRequestDto request);
    Task<ReadShowUiDto?> GetSingleShow(string id);
    Task<ReadShowUiDto> CreateShow(CreateShowDto showCreate);
    Task UpdateShow(UpdateShowDto showUpdate);
    Task DeleteShow(string showId);
    Task CreateOrUpdateShowSchedule(UpdateShowScheduleDto updateShowSchedule);
    Task DeleteShowSchedule(string showId);
}