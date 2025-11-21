using FluentValidation;
using SimpleBlogApi.Application.Commands.BlogPosts;

namespace SimpleBlogApi.Application.Validators.BlogPosts;

public class GetBlogPostDetailCommandValidator
    : AbstractValidator<GetBlogPostDetailCommand>
{
    public GetBlogPostDetailCommandValidator()
    {
        RuleFor(_ => _.Id)
        .GreaterThan(0)
        .WithMessage("Post ID must be greater than zero.");
    }
}