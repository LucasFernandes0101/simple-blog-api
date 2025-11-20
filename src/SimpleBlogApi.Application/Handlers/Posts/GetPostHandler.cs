using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.Mappers.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.Posts;

public class GetPostHandler(
    IPostRepository postRepository,
    IValidator<GetPostCommand> validator)
    : IRequestHandler<GetPostCommand, PagedResult<GetPostResult>>
{
    public async Task<PagedResult<GetPostResult>> Handle(
        GetPostCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateAsync(command, cancellationToken);

        var pagedResult = await postRepository.GetWithCommentsAsync(
            command.Page,
            command.Size,
            default,
            cancellationToken
        );

        return new PagedResult<GetPostResult>(
            pagedResult.Total,
            pagedResult.Items.ToResult()
        );
    }

    private async Task ValidateAsync(
        GetPostCommand command, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(
            command,
            cancellationToken
        );

        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);
    }
}