namespace SimpleBlogApi.Application.DTOs.Comments;

public record GetCommentResponseDTO(
    int Id,
    string Content,
    string Author,
    DateTimeOffset CreatedAt);