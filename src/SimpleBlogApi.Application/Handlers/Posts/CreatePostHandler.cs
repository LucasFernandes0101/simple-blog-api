using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.Mappers.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.Posts;

public class CreatePostHandler(
    IPostRepository postRepository,
    IValidator<CreatePostCommand> validator) 
    : IRequestHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> Handle(
        CreatePostCommand command, 
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
        CreatePostCommand command, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(
            command, 
            cancellationToken
        );

        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);
    }
}