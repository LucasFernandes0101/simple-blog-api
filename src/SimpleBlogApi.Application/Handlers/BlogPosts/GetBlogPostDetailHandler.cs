using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class GetBlogPostDetailHandler(
    IBlogPostRepository postRepository)
    : IRequestHandler<GetBlogPostDetailCommand, GetBlogPostDetailResult?>
{
    public async Task<GetBlogPostDetailResult?> Handle(
        GetBlogPostDetailCommand command, 
        CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdWithCommentsAsync(
            command.Id,
            cancellationToken
        );

        return post?.ToDetailResult();
    }
}