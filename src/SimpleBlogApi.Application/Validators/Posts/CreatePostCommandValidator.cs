using FluentValidation;
using SimpleBlogApi.Application.Commands.Posts;

namespace SimpleBlogApi.Application.Validators.Posts;

public class CreatePostCommandValidator
    : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(_ => _.Title).NotEmpty().Length(3, 200);
        RuleFor(_ => _.Content).NotEmpty();
    }
}