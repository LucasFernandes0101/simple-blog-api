namespace SimpleBlogApi.Application.DTOs.Posts;

public record GetPostResponseDTO(
    int Id,
    string Title,
    string Content,
    int CommentCount);