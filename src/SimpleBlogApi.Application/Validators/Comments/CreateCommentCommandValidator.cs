using FluentValidation;
using SimpleBlogApi.Application.Commands.Comments;

namespace SimpleBlogApi.Application.Validators.Comments;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0)
            .WithMessage("PostId must be a positive integer.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(1000)
            .WithMessage("Content must be at most 1000 characters.");

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Author is required.")
            .MaximumLength(100)
            .WithMessage("Author must be at most 100 characters.");
    }
}