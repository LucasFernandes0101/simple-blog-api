namespace SimpleBlogApi.Application.Results.Comments;

public record GetCommentResult(
    string Content,
    string Author);