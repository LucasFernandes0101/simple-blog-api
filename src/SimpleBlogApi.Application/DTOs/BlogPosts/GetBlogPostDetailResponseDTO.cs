using SimpleBlogApi.Application.DTOs.Comments;

namespace SimpleBlogApi.Application.DTOs.Posts;

public record GetBlogPostDetailResponseDTO
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public List<GetCommentResponseDTO> Comments { get; init; } = new();
    public DateTimeOffset CreatedAt { get; init; }
}