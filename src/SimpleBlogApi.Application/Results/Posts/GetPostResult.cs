namespace SimpleBlogApi.Application.Results.Posts;

public record GetPostResult(
    int Id,
    string Title,
    string Content,
    int CommentCount);