using Microsoft.AspNetCore.Mvc;
using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Web.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpGet]
    public ActionResult<UserInfo> UserInfo()
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        return Ok(new UserInfo
        {
            IsAuthenticated = isAuthenticated,
            UserName = User.Identity?.Name,
            Claims = isAuthenticated ? User.Claims.ToDictionary(c => c.Type, c => c.Value) : null
        });
    }
}
