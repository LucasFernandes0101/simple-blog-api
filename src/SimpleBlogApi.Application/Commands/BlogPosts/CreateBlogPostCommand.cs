using MediatR;
using SimpleBlogApi.Application.Results.BlogPosts;

namespace SimpleBlogApi.Application.Commands.BlogPosts;

public record CreateBlogPostCommand(
    string Title,
    string Content) : IRequest<CreateBlogPostResult>;