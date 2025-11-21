using MediatR;
using SimpleBlogApi.Application.Results.BlogPosts;

namespace SimpleBlogApi.Application.Commands.BlogPosts;

public record GetBlogPostDetailCommand(
    int Id) : IRequest<GetBlogPostDetailResult?>;