namespace SimpleBlogApi.Application.Results.Posts;

public record GetPostResult
{
    public GetPostResult() { }

    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public int CommentCount { get; init; }
}