namespace SimpleBlogApi.Domain.Base;

public class PagedResult<T>(int total, List<T> items)
{
    public int Total { get; set; } = total;

    public List<T> Items { get; set; } = items;
}