using FluentValidation;
using MediatR;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Mappers.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Interfaces.Repositories;

namespace SimpleBlogApi.Application.Handlers.BlogPosts;

public class GetBlogPostDetailHandler(
    IBlogPostRepository postRepository,
    IValidator<GetBlogPostDetailCommand> validator)
    : IRequestHandler<GetBlogPostDetailCommand, GetBlogPostDetailResult?>
{
    public async Task<GetBlogPostDetailResult?> Handle(
        GetBlogPostDetailCommand command, 
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
        GetBlogPostDetailCommand command,
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