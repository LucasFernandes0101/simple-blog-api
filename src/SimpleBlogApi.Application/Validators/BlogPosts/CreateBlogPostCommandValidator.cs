using FluentValidation;
using SimpleBlogApi.Application.Commands.Posts;

namespace SimpleBlogApi.Application.Validators.Posts;

public class CreateBlogPostCommandValidator
    : AbstractValidator<CreatePostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(_ => _.Title).NotEmpty().Length(3, 200).WithMessage("The title should be between 3 and 200 characters.");
        RuleFor(_ => _.Content).NotEmpty().WithMessage("The content cannot be empty.");
    }
}