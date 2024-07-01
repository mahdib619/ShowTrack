using ShowTrack.Client.Models.Dtos;
using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Services;

public interface IShowsService
{
    Task<PagedResponseDto<ReadShowDto>?> GetAllShows(PagedRequestDto request);
    Task<ReadShowDto?> GetSingleShow(string id);
    Task<ReadShowDto> CreateShow(CreateShowDto showCreate);
    Task UpdateShow(UpdateShowDto showUpdate);
    Task DeleteShow(string showId);
    Task CreateOrUpdateShowSchedule(UpdateShowScheduleDto updateShowSchedule);
    Task DeleteShowSchedule(string showId);
}