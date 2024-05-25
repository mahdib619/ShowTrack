using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
