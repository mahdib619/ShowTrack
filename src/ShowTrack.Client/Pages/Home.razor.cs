using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ShowTrack.Client.Models.Dtos;
using ShowTrack.Client.Shared;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Domain.Entities;

namespace ShowTrack.Client.Pages;

public partial class Home
{
    private const int PAGE_SIZE = 6;
    private readonly List<string> _sortItems = [nameof(Show.DateAdded) + "Desc"];
    private readonly DialogOptions _defaultDialogOptions = new() { CloseDialogOnOverlayClick = true };

    private bool _isLoading;
    private bool _onlyDisplayOnGoingShows;

    private RadzenDataList<ReadShowUiDto> _showDataList = new();

    private int _showsCount;
    private PagedResponseDto<ReadShowUiDto>? _pagedShows;
    private ICollection<ReadShowUiDto>? _shows;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _onlyDisplayOnGoingShows = await LocalStorage.GetItemAsync<bool>(nameof(_onlyDisplayOnGoingShows));
            await DisplayFilterChanged(_onlyDisplayOnGoingShows);
        }
    }

    private async Task LoadData(LoadDataArgs loadArgs)
    {
        _isLoading = true;

        var request = new PagedRequestDto
        {
            Page = (loadArgs.Skip / loadArgs.Top) + 1,
            Count = loadArgs.Top,
            FilterObject = _onlyDisplayOnGoingShows ? new { IsEnded = false } : null,
            SortItems = _sortItems
        };

        _pagedShows = await ShowsService.GetAllShows(request);
        _shows = _pagedShows?.Items;
        _showsCount = _pagedShows?.TotalCount ?? 0;

        _isLoading = false;
    }

    private async Task AddShow()
    {
        CreateShowDto? newShow = await DialogService.OpenAsync<AddShowPage>("New Show", null, _defaultDialogOptions);

        if (newShow is null)
        {
            return;
        }

        await ShowsService.CreateShow(newShow);

        await _showDataList.Reload();
    }

    private async Task UpdateShow(ReadShowUiDto show)
    {
        show = await DialogService.OpenAsync<UpdateShowPage>("Update Show", new() { ["Show"] = show }, _defaultDialogOptions);

        if (show is null)
        {
            return;
        }

        await ShowsService.UpdateShow(UpdateShowDto.FromReadDto(show));

        await _showDataList.Reload();
    }

    private async Task OnShowRatingChange(ReadShowUiDto show, int newRating)
    {
        show.PersonalRating = newRating;
        await ShowsService.UpdateShow(UpdateShowDto.FromReadDto(show));
    }

    private async Task AddSchedule(ReadShowUiDto show)
    {
        var title = show.Schedule is null ? "Add Schedule" : "Update Schedule";
        UpdateShowScheduleDto? newSchedule = await DialogService.OpenAsync<AddSchedulePage>(title, new() { ["Show"] = show }, _defaultDialogOptions);

        if (newSchedule is null)
        {
            return;
        }

        await ShowsService.CreateOrUpdateShowSchedule(newSchedule);

        show.Schedule = (await ShowsService.GetSingleShow(show.Id))?.Schedule;
    }

    private void PromptDelete(ReadShowUiDto show)
    {
        TooltipService.Close();
        show.ShowDeletePrompt = true;
    }

    private static void CancelDelete(ReadShowUiDto show)
    {
        show.ShowDeletePrompt = false;
    }

    private async Task DeleteShow(ReadShowUiDto show)
    {
        await ShowsService.DeleteShow(show.Id);
        await _showDataList.Reload();
    }

    private void ShowTooltip(ElementReference elementReference, string text, string cssClass)
    {
        TooltipService.Open(elementReference, text, new() { Position = TooltipPosition.Top, CssClass = cssClass });
    }

    private async Task DisplayFilterChanged(bool value)
    {
        _onlyDisplayOnGoingShows = value;
        await LocalStorage.SetItemAsync(nameof(_onlyDisplayOnGoingShows), _onlyDisplayOnGoingShows);

        _showDataList.CurrentPage = 0;
        await _showDataList.Reload();
    }
}