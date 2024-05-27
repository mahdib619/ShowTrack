using OneOf;
using ShowTrack.Web.Models;
using ShowTrack.Web.Models.Dtos;

namespace ShowTrack.Web.Services;

public interface IShowService
{
    Task<IReadOnlyCollection<ReadShowDto>> GetAllUserShows(string userId);
    Task<ReadShowDto?> GetShow(string userId, string showId);
    Task<ReadShowDto> CreateShow(CreateShowDto showCreate);
    Task<OneOf<bool, ClientError>> UpdateShow(string userId, UpdateShowDto showUpdate);
    Task<OneOf<bool, ClientError>> DeleteShow(string userId, string showId);
}