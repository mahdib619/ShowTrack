using Coravel.Invocable;
using ShowTrack.Domain.Entities;
using ShowTrack.Web.Services;

namespace ShowTrack.Web.Jobs;

public sealed class NotifyShowsNewSeasonJob(IShowService showService, IEmailService emailService) : IInvocable
{
    private const string NOTIFY_SHOW_TEMPLATE = "Season {0} of {1} is relased today, DON'T MISS IT!";

    public async Task Invoke()
    {
        var todayShows = await showService.GetTodayShows();

        await Task.WhenAll(todayShows.Select(Notify));

        await showService.DeleteSchedulesAndUpdateShowSeason(todayShows.Select(s => s.Schedule!.Id).ToArray());

        async Task Notify(Show show)
        {
            if (show.User?.Email is not null)
            {
                await emailService.Send(show.User.Email, "New season notification", string.Format(NOTIFY_SHOW_TEMPLATE, show.Schedule!.Season, show.Title));
            }
        }
    }
}
