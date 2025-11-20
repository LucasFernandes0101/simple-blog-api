using SimpleBlogApi.Application.DTOs.Comments;

namespace SimpleBlogApi.Application.DTOs.Posts;

public record GetPostDetailResponseDTO(
    int Id,
    string Title,
    string Content,
    List<GetCommentResponseDTO> Comments);