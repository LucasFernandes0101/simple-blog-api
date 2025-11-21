using MediatR;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Application.Commands.BlogPosts;

public record GetBlogPostCommand(
    int Page,
    int Size) : IRequest<PagedResult<GetBlogPostResult>>;