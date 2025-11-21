namespace SimpleBlogApi.Application.DTOs.Posts;

public record GetBlogPostResponseDTO(
    int Id,
    string Title,
    string Content,
    int CommentCount);