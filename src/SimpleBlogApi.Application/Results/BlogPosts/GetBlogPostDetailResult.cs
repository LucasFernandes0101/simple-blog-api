using SimpleBlogApi.Application.Results.Comments;

namespace SimpleBlogApi.Application.Results.Posts;

public record GetBlogPostDetailResult(
    int Id,
    string Title,
    string Content,
    List<GetCommentResult> Comments,
    DateTimeOffset CreatedAt);