using Newtonsoft.Json;

namespace ShowTrack.Client.Models.Dtos;

public class PagedRequestDto
{
    public int? Page { get; init; }
    public int? Count { get; init; }

    private object? _filterObject;

    public object? FilterObject
    {
        get => _filterObject;
        set
        {
            if (value is not null)
            {
                Filter = JsonConvert.SerializeObject(value);
            }

            _filterObject = value;
        }
    }

    public string? Filter { get; private set; }

    private ICollection<string>? _sortItems;
    public ICollection<string>? SortItems
    {
        get => _sortItems;
        set
        {
            if (value is not null)
            {
                Sort = JsonConvert.SerializeObject(value);
            }
            _sortItems = value;
        }
    }

    public string? Sort { get; private set; }

    public string ToQueryString()
    {
        var queryDict = new Dictionary<string, object>();

        if (Page is not null && Count is not null)
        {
            queryDict[nameof(Page)] = Page;
            queryDict[nameof(Count)] = Count;
        }

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            queryDict[nameof(Filter)] = Filter;
        }

        if (!string.IsNullOrWhiteSpace(Sort))
        {
            queryDict[nameof(Sort)] = Sort;
        }

        if (queryDict.Count == 0)
        {
            return string.Empty;
        }

        var queryParams = queryDict.Select(kv => $"{kv.Key}={kv.Value}");

        return $"?{string.Join('&', queryParams)}";
    }
}