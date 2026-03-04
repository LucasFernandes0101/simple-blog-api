using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class CreateBlogPostHandler(
    IBlogPostRepository postRepository)
    : IRequestHandler<CreateBlogPostCommand, CreateBlogPostResult>
{
    public async Task<CreateBlogPostResult> Handle(
        CreateBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        var post = command.ToEntity();

        var createdPost = await postRepository.AddAsync(
            post, 
            cancellationToken
        );

        return createdPost.ToCreateResult();
    }
}