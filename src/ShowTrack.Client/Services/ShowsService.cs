using ShowTrack.Client.Models.Dtos;
using ShowTrack.Contracts.Dtos;
using System.Net.Http.Json;

namespace ShowTrack.Client.Services;

public sealed class ShowsService(HttpClient httpClient) : IShowsService
{
    public async Task<PagedResponseDto<ReadShowDto>?> GetAllShows(PagedRequestDto request)
    {
        return await httpClient.GetFromJsonAsync<PagedResponseDto<ReadShowDto>>("api/Shows" + request.ToQueryString());
    }

    public async Task<ReadShowDto?> GetSingleShow(string id)
    {
        return await httpClient.GetFromJsonAsync<ReadShowDto>($"api/Shows/{id}");
    }

    public async Task<ReadShowDto> CreateShow(CreateShowDto showCreate)
    {
        using var result = await httpClient.PostAsJsonAsync("api/Shows", showCreate);
        return (await result.Content.ReadFromJsonAsync<ReadShowDto>())!;
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
