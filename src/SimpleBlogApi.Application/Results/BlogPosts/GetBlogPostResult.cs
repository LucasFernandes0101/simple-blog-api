namespace SimpleBlogApi.Application.Results.BlogPosts;

public record GetBlogPostResult
{
    public GetBlogPostResult() { }

    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public int CommentCount { get; init; }
}