using FluentValidation;
using SimpleBlogApi.Application.Commands.BlogPosts;

namespace SimpleBlogApi.Application.Validators.BlogPosts;

public class CreateBlogPostCommandValidator
    : AbstractValidator<CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(_ => _.Title).NotEmpty().Length(3, 200).WithMessage("The title should be between 3 and 200 characters.");
        RuleFor(_ => _.Content).NotEmpty().WithMessage("The content cannot be empty.");
    }
}