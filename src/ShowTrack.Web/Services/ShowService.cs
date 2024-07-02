using Microsoft.EntityFrameworkCore;
using OneOf;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Data;
using ShowTrack.Domain.Entities;
using ShowTrack.Web.Extensions;
using ShowTrack.Web.Models;
using ShowTrack.Web.Models.Dtos;

namespace ShowTrack.Web.Services;

public sealed class ShowService(AppDbContext dbContext) : IShowService
{
    public static TimeOnly ShowsNotifyTime { get; } = new(10, 0, 0);

    public async Task<PagedResponseDto<ReadShowDto>> GetAllUserShows<TFilter>(PagedRequestDto<TFilter> request) where TFilter : class
    {
        var response = await dbContext.Shows.Include(s => s.Schedule)
                                            .OrderByDescending(s => s.DateAdded)
                                            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall; "It is convertible!"
                                            .FilterAndPaginate(request, s => ReadShowDto.FromEntity(s));

        return response;
    }

    public async Task<ReadShowDto?> GetShow(string userId, string showId)
    {
        var show = await dbContext.Shows.Include(s => s.Schedule)
                                        .Where(s => s.UserId == userId && s.Id == showId)
                                        .Select(s => ReadShowDto.FromEntity(s))
                                        .FirstOrDefaultAsync();

        return show;
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
                StatusCode = StatusCodes.Status404NotFound
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
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        dbContext.Shows.Remove(show);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<OneOf<ReadShowScheduleDto, bool, ClientError>> CreateOrUpdateShowSchedule(string userId, UpdateShowScheduleDto updateShowSchedule)
    {
        var show = await dbContext.Shows.Include(s => s.Schedule)
                                        .Where(s => s.UserId == userId && s.Id == updateShowSchedule.ShowId)
                                        .FirstOrDefaultAsync();

        if (show is null)
        {
            return new ClientError
            {
                Message = "Invalid show id!",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        if (show.Schedule is null)
        {
            show.Schedule = updateShowSchedule.ToEntity();
            await dbContext.SaveChangesAsync();
            return ReadShowScheduleDto.FromEntity(show.Schedule);
        }

        updateShowSchedule.UpdateEntity(show.Schedule);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<OneOf<bool, ClientError>> DeleteShowSchedule(string userId, string showId)
    {
        var show = await dbContext.Shows.Include(s => s.Schedule)
                                        .Where(s => s.UserId == userId && s.Id == showId)
                                        .FirstOrDefaultAsync();

        if (show is null)
        {
            return new ClientError
            {
                Message = "Invalid show id!",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        if (show.Schedule is null)
        {
            return new ClientError
            {
                Message = "Show has no schedule!",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        show.Schedule = null;

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task DeleteSchedulesAndUpdateShowSeason(string[] ids)
    {
        var schedules = await dbContext.ShowSchedules.Include(s => s.Show)
                                                     .Where(s => ids.Contains(s.Id))
                                                     .ToListAsync();


        foreach (var showSchedule in schedules)
        {
            showSchedule.Show!.CurrentSeason = showSchedule.Season;
        }

        dbContext.ShowSchedules.RemoveRange(schedules);

        await dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Show>> GetTodayShows()
    {
        var shows = await dbContext.Shows.Include(s => s.Schedule)
                                         .Include(s => s.User)
                                         .Where(s => !s.IsEnded && s.Schedule != null && s.Schedule.ReleaseDate == DateOnly.FromDateTime(DateTime.Today))
                                         .AsNoTracking()
                                         .ToListAsync();

        return shows;
    }
}
