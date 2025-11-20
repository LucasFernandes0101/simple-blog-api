using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.Mappers.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.Posts;

public class GetPostDetailHandler(
    IPostRepository postRepository,
    IValidator<GetPostDetailCommand> validator)
    : IRequestHandler<GetPostDetailCommand, GetPostDetailResult?>
{
    public async Task<GetPostDetailResult?> Handle(
        GetPostDetailCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateAsync(command, cancellationToken);

        var post = await postRepository.GetByIdWithCommentsAsync(
            command.Id,
            cancellationToken
        );

        return post?.ToDetailResult();
    }

    private async Task ValidateAsync(
        GetPostDetailCommand command,
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