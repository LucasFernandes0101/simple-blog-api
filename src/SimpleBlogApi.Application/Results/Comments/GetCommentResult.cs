namespace SimpleBlogApi.Application.Results.Comments;

public record GetCommentResult(
    int Id,
    string Content,
    string Author,
    DateTimeOffset CreatedAt);