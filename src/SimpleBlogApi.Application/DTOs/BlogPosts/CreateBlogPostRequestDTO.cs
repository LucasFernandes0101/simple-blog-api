namespace SimpleBlogApi.Application.DTOs.BlogPosts;

public record CreateBlogPostRequestDTO(
    string Title,
    string Content
);