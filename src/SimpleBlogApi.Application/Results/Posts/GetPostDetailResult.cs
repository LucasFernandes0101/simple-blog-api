using SimpleBlogApi.Application.Results.Comments;

namespace SimpleBlogApi.Application.Results.Posts;

public record GetPostDetailResult(
    int Id,
    string Title,
    string Content,
    List<GetCommentResult> Comments,
    DateTimeOffset CreatedAt);