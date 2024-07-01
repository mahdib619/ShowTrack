using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Web.Extensions;
using ShowTrack.Web.Models.Dtos;
using ShowTrack.Web.Services;
using System.Security.Claims;

namespace ShowTrack.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public sealed class ShowsController(IShowService showService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ReadShowDto>>> GetAllShows([FromQuery] PagedRequestDto<JObject> request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        request.AddFilter("UserId", userId);

        var shows = await showService.GetAllUserShows(request);
        return Ok(shows);
    }

    [HttpGet("{id}", Name = nameof(GetShow))]
    public async Task<ActionResult<ReadShowDto>> GetShow(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var show = await showService.GetShow(userId, id);
        return Ok(show);
    }

    [HttpPost("testData")]
    public async Task<ActionResult> TestData()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        for (var i = 0; i < 100; i++)
        {
            await showService.CreateShow(new() { CurrentSeason = i, Title = "TestShow", UserId = userId });
        }

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ReadShowDto>> CreateShow(CreateShowDto createShow)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        createShow.UserId = userId;

        var show = await showService.CreateShow(createShow);

        return CreatedAtAction(nameof(GetShow), new { show.Id }, show);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShow(string id, UpdateShowDto updateShow)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        updateShow.Id = id;

        var result = await showService.UpdateShow(userId, updateShow);

        return result.Match(this.HandleEntityChange, this.HandleClientError);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShow(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await showService.DeleteShow(userId, id);

        return result.Match(this.HandleEntityChange, this.HandleClientError);
    }

    [HttpPut("{showId}/schedule")]
    public async Task<IActionResult> CreateOrUpdateShowSchedule(string showId, UpdateShowScheduleDto updateShowSchedule)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        updateShowSchedule.ShowId = showId;

        var result = await showService.CreateOrUpdateShowSchedule(userId, updateShowSchedule);

        return result.Match(HandleShowScheduleCreated, this.HandleEntityChange, this.HandleClientError);

        IActionResult HandleShowScheduleCreated(ReadShowScheduleDto showSchedule)
        {
            return CreatedAtAction(nameof(GetShow), new { Id = showSchedule.ShowId }, showSchedule);
        }
    }

    [HttpDelete("{showId}/schedule")]
    public async Task<IActionResult> DeleteShowSchedule(string showId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await showService.DeleteShowSchedule(userId, showId);

        return result.Match(this.HandleEntityChange, this.HandleClientError);
    }
}
