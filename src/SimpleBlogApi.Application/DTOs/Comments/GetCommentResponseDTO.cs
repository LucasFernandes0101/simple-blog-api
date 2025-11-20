namespace SimpleBlogApi.Application.DTOs.Comments;

public record GetCommentResponseDTO(
    string Content,
    string Author);