using FluentValidation;
using SimpleBlogApi.Application.Commands.Posts;

namespace SimpleBlogApi.Application.Validators.Posts;

public class GetBlogPostDetailCommandValidator
    : AbstractValidator<GetPostDetailCommand>
{
    public GetBlogPostDetailCommandValidator()
    {
        RuleFor(_ => _.Id)
        .GreaterThan(0)
        .WithMessage("Post ID must be greater than zero.");
    }
}