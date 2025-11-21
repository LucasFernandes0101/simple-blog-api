using MediatR;
using SimpleBlogApi.Application.Results.Posts;

namespace SimpleBlogApi.Application.Commands.Posts;

public record CreatePostCommand(
    string Title,
    string Content) : IRequest<CreateBlogPostResult>;