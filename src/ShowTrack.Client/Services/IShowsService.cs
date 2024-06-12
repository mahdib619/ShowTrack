using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Services;

public interface IShowsService
{
    Task<IList<ReadShowDto>?> GetAllShows();
    Task<ReadShowDto?> GetSingleShow(string id);
    Task<ReadShowDto> CreateShow(CreateShowDto showCreate);
    Task UpdateShow(UpdateShowDto showUpdate);
    Task DeleteShow(string showId);
    Task CreateOrUpdateShowSchedule(UpdateShowScheduleDto updateShowSchedule);
    Task DeleteShowSchedule(string showId);
}