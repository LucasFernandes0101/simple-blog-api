namespace SimpleBlogApi.Application.DTOs.Posts;

public record CreateBlogPostRequestDTO(
    string Title,
    string Content
);