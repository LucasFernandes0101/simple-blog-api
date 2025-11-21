using FluentValidation;
using SimpleBlogApi.Application.Commands.Posts;

namespace SimpleBlogApi.Application.Validators.Posts;

public class GetBlogPostCommandValidator 
    : AbstractValidator<GetPostCommand> 
{
	public GetBlogPostCommandValidator()
	{
		RuleFor(_ => _.Page).GreaterThan(0).WithMessage("Page number must be greater than 0.");
		RuleFor(_ => _.Size).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}