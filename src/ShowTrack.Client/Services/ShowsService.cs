using ShowTrack.Client.Models.Dtos;
using ShowTrack.Contracts.Dtos;
using System.Net.Http.Json;

namespace ShowTrack.Client.Services;

public sealed class ShowsService(HttpClient httpClient) : IShowsService
{
    public async Task<PagedResponseDto<ReadShowUiDto>?> GetAllShows(PagedRequestDto request)
    {
        return await httpClient.GetFromJsonAsync<PagedResponseDto<ReadShowUiDto>>("api/Shows" + request.ToQueryString());
    }

    public async Task<ReadShowUiDto?> GetSingleShow(string id)
    {
        return await httpClient.GetFromJsonAsync<ReadShowUiDto>($"api/Shows/{id}");
    }

    public async Task<ReadShowUiDto> CreateShow(CreateShowDto showCreate)
    {
        using var result = await httpClient.PostAsJsonAsync("api/Shows", showCreate);
        return (await result.Content.ReadFromJsonAsync<ReadShowUiDto>())!;
    }

    public async Task UpdateShow(UpdateShowDto showUpdate)
    {
        using var _ = await httpClient.PutAsJsonAsync($"api/Shows/{showUpdate.Id}", showUpdate);
    }

    public async Task DeleteShow(string showId)
    {
        using var _ = await httpClient.DeleteAsync($"api/Shows/{showId}");
    }

    public async Task CreateOrUpdateShowSchedule(UpdateShowScheduleDto updateShowSchedule)
    {
        using var _ = await httpClient.PutAsJsonAsync($"api/Shows/{updateShowSchedule.ShowId}/schedule", updateShowSchedule);
    }

    public async Task DeleteShowSchedule(string showId)
    {
        using var _ = await httpClient.DeleteAsync($"api/Shows/{showId}/schedule");
    }
}
