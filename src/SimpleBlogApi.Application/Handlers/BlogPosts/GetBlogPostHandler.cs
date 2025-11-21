using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class GetBlogPostHandler(
    IBlogPostRepository postRepository,
    IValidator<GetBlogPostCommand> validator)
    : IRequestHandler<GetBlogPostCommand, PagedResult<GetBlogPostResult>>
{
    public async Task<PagedResult<GetBlogPostResult>> Handle(
        GetBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateAsync(command, cancellationToken);

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

    private async Task ValidateAsync(
        GetBlogPostCommand command, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(
            command,
            cancellationToken
        );

        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);
    }
}