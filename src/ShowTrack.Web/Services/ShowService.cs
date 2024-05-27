using Microsoft.EntityFrameworkCore;
using OneOf;
using ShowTrack.Data;
using ShowTrack.Web.Models;
using ShowTrack.Web.Models.Dtos;

namespace ShowTrack.Web.Services;

public sealed class ShowService(AppDbContext dbContext) : IShowService
{
    public async Task<IReadOnlyCollection<ReadShowDto>> GetAllUserShows(string userId)
    {
        var shows = await dbContext.Shows.Where(s => s.UserId == userId)
                                         .Select(s => ReadShowDto.FromEntity(s))
                                         .ToListAsync();

        return shows;
    }

    public async Task<ReadShowDto?> GetShow(string userId, string showId)
    {
        var shows = await dbContext.Shows.Where(s => s.UserId == userId && s.Id == showId)
                                         .Select(s => ReadShowDto.FromEntity(s))
                                         .FirstOrDefaultAsync();

        return shows;
    }

    public async Task<ReadShowDto> CreateShow(CreateShowDto showCreate)
    {
        var show = showCreate.ToEntity();
        await dbContext.AddAsync(show);

        await dbContext.SaveChangesAsync();

        return ReadShowDto.FromEntity(show);
    }

    public async Task<OneOf<bool, ClientError>> UpdateShow(string userId, UpdateShowDto showUpdate)
    {
        var show = await dbContext.Shows.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == showUpdate.Id);

        if (show is null)
        {
            return new ClientError
            {
                Message = "Show not found!",
                StatusCode = 404
            };
        }

        showUpdate.UpdateEntity(show);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<OneOf<bool, ClientError>> DeleteShow(string userId, string showId)
    {
        var show = await dbContext.Shows.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == showId);

        if (show is null)
        {
            return new ClientError
            {
                Message = "Show not found!",
                StatusCode = 404
            };
        }

        dbContext.Shows.Remove(show);

        return await dbContext.SaveChangesAsync() > 0;
    }
}
