namespace SimpleBlogApi.Application.DTOs.BlogPosts;

public record GetBlogPostResponseDTO(
    int Id,
    string Title,
    string Content,
    int CommentCount);