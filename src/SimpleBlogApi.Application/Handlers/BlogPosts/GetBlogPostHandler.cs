using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class GetBlogPostHandler(
    IBlogPostRepository postRepository)
    : IRequestHandler<GetBlogPostCommand, PagedResult<GetBlogPostResult>>
{
    public async Task<PagedResult<GetBlogPostResult>> Handle(
        GetBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        var pagedResult = await postRepository.GetWithCommentsAsync(
            command.Page,
            command.Size,
            default,
            cancellationToken
        );

        return new PagedResult<GetBlogPostResult>(
            pagedResult.Total,
            pagedResult.Items.ToResult()
        );
    }
}