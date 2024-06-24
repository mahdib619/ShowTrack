namespace ShowTrack.Contracts.Dtos;

public class PagedResponseDto<TItem>(ICollection<TItem> items, int totalCount, int page)
{
    public PagedResponseDto() : this([], 0, 0)
    {

    }

    public int Page { get; init; } = page;
    public int TotalCount { get; init; } = totalCount;
    public ICollection<TItem> Items { get; init; } = items;
}
