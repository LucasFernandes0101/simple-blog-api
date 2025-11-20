namespace SimpleBlogApi.Application.DTOs.Posts;

public record CreatePostRequestDTO(
    string Title,
    string Content
);