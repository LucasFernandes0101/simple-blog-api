using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.Mappers.Comments;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Domain.Exceptions;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.Comments;

public class CreateCommentHandler(
    ICommentRepository commentRepository,
    IBlogPostRepository postRepository,
    IValidator<CreateCommentCommand> validator)
    : IRequestHandler<CreateCommentCommand, CreateCommentResult>
{
    public async Task<CreateCommentResult> Handle(
        CreateCommentCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateAsync(command, cancellationToken);

        var comment = command.ToEntity();

        var createdComment = await commentRepository.AddAsync(
            comment, 
            cancellationToken
        );

        return createdComment.ToCreateResult();
    }

    private async Task ValidateAsync(
        CreateCommentCommand command, 
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(
            command,
            cancellationToken
        );

        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        await EnsurePostExists(command.BlogPostId, cancellationToken);
    }

    private async Task EnsurePostExists(int blogBlogPostId, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(blogBlogPostId, cancellationToken);

        if (post is null)
            throw new BlogPostNotFoundException($"Post with ID {blogBlogPostId} not found.");
    }
}
