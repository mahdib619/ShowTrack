using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowTrack.Web.Extensions;
using ShowTrack.Web.Models.Dtos;
using ShowTrack.Web.Services;
using System.Security.Claims;

namespace ShowTrack.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ShowsController(IShowService showService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ReadShowDto>>> GetAllShows()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var shows = await showService.GetAllUserShows(userId);
        return Ok(shows);
    }

    [HttpGet("{id}", Name = nameof(GetShow))]
    public async Task<ActionResult<ReadShowDto>> GetShow(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var show = await showService.GetShow(userId, id);
        return Ok(show);
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
}
