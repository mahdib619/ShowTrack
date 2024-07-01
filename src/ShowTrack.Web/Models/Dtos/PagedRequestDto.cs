using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace ShowTrack.Web.Models.Dtos;

public class PagedRequestDto<TFilter> where TFilter : class
{
    public int? Page { get; init; }
    public int? Count { get; init; }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            var json = string.IsNullOrWhiteSpace(value) ? "{}" : value;

            FilterObj = JsonConvert.DeserializeObject<TFilter>(json);

            _filter = value;
        }
    }

    [SwaggerIgnore]
    public TFilter? FilterObj { get; private set; }
}