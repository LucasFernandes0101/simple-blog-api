using FluentValidation;
using SimpleBlogApi.Application.Commands.Posts;

namespace SimpleBlogApi.Application.Validators.Posts;

public class GetPostDetailCommandValidator
    : AbstractValidator<GetPostDetailCommand>
{
    public GetPostDetailCommandValidator()
    {
        RuleFor(_ => _.Id)
        .GreaterThan(0)
        .WithMessage("Post ID must be greater than zero.");
    }
}