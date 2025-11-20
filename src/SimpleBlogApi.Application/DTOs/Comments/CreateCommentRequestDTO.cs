namespace SimpleBlogApi.Application.DTOs.Comments;

public record CreateCommentRequestDTO(
    string Content,
    string Author);