namespace SimpleBlogApi.Application.Extensions;

public static class EnumerableExtensions
{
    public static bool AnySafe<T>(this IEnumerable<T>? source)
        => source is not null && source.Any();
}