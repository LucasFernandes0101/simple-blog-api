using MediatR;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.Mappers.Comments;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Domain.Exceptions;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.Comments;

public class CreateCommentHandler(
    ICommentRepository commentRepository,
    IBlogPostRepository postRepository)
    : IRequestHandler<CreateCommentCommand, CreateCommentResult>
{
    public async Task<CreateCommentResult> Handle(
        CreateCommentCommand command, 
        CancellationToken cancellationToken)
    {
        await EnsurePostExists(command.BlogPostId, cancellationToken);

        var comment = command.ToEntity();

        var createdComment = await commentRepository.AddAsync(
            comment, 
            cancellationToken
        );

        return createdComment.ToCreateResult();
    }

    private async Task EnsurePostExists(int blogBlogPostId, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(blogBlogPostId, cancellationToken);

        if (post is null)
            throw new BlogPostNotFoundException($"Post with ID {blogBlogPostId} not found.");
    }
}
