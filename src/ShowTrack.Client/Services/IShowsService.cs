using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Services;

public interface IShowsService
{
    Task<PagedResponseDto<ReadShowDto>?> GetAllShows(int? page, int? count);
    Task<ReadShowDto?> GetSingleShow(string id);
    Task<ReadShowDto> CreateShow(CreateShowDto showCreate);
    Task UpdateShow(UpdateShowDto showUpdate);
    Task DeleteShow(string showId);
    Task CreateOrUpdateShowSchedule(UpdateShowScheduleDto updateShowSchedule);
    Task DeleteShowSchedule(string showId);
}