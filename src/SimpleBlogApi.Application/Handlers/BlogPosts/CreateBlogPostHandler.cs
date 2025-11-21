using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class CreateBlogPostHandler(
    IBlogPostRepository postRepository,
    IValidator<CreateBlogPostCommand> validator) 
    : IRequestHandler<CreateBlogPostCommand, CreateBlogPostResult>
{
    public async Task<CreateBlogPostResult> Handle(
        CreateBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateAsync(command, cancellationToken);

        var post = command.ToEntity();

        var createdPost = await postRepository.AddAsync(
            post, 
            cancellationToken
        );

        return createdPost.ToCreateResult();
    }

    private async Task ValidateAsync(
        CreateBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(
            command, 
            cancellationToken
        );

        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);
    }
}