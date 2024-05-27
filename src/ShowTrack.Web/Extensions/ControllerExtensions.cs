using Microsoft.AspNetCore.Mvc;
using ShowTrack.Web.Models;

namespace ShowTrack.Web.Extensions;

public static class ControllerExtensions
{
    private static readonly ApiError _unknownError = new() { Message = "Something went wrong!", StatusCode = StatusCodes.Status500InternalServerError };

    public static IActionResult HandleClientError(this ControllerBase controller, ClientError clientError)
    {
        return controller.StatusCode(clientError.StatusCode, clientError);
    }

    public static IActionResult HandleEntityChange(this ControllerBase controller, bool changeResult) => changeResult switch
    {
        true => controller.NoContent(),
        false => controller.StatusCode(StatusCodes.Status500InternalServerError, _unknownError)
    };

}
