using Newtonsoft.Json.Linq;
using ShowTrack.Web.Models.Dtos;

namespace ShowTrack.Web.Extensions;

public static class PagedRequestExtensions
{
    public static void AddFilter(this PagedRequestDto<JObject> request, string name, object value)
    {
        if (request.FilterObj is null)
        {
            request.Filter = "{}";
        }

        request.FilterObj!.Add(name, JToken.FromObject(value));
    }
}
