using OneOf;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Domain.Entities;
using ShowTrack.Web.Models;
using ShowTrack.Web.Models.Dtos;

namespace ShowTrack.Web.Services;

public interface IShowService
{
    Task<PagedResponseDto<ReadShowDto>> GetAllUserShows<TFilter>(PagedRequestDto<TFilter> request) where TFilter : class;
    Task<ReadShowDto?> GetShow(string userId, string showId);
    Task<ReadShowDto> CreateShow(CreateShowDto showCreate);
    Task<OneOf<bool, ClientError>> UpdateShow(string userId, UpdateShowDto showUpdate);
    Task<OneOf<bool, ClientError>> DeleteShow(string userId, string showId);
    Task<OneOf<ReadShowScheduleDto, bool, ClientError>> CreateOrUpdateShowSchedule(string userId, UpdateShowScheduleDto updateShowSchedule);
    Task<OneOf<bool, ClientError>> DeleteShowSchedule(string userId, string showId);
    Task<IReadOnlyList<Show>> GetTodayShows();
    Task DeleteSchedulesAndUpdateShowSeason(string[] ids);
}