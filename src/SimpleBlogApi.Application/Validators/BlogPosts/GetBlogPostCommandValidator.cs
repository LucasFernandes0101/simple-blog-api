using FluentValidation;
using SimpleBlogApi.Application.Commands.BlogPosts;

namespace SimpleBlogApi.Application.Validators.BlogPosts;

public class GetBlogPostCommandValidator 
    : AbstractValidator<GetBlogPostCommand> 
{
	public GetBlogPostCommandValidator()
	{
		RuleFor(_ => _.Page).GreaterThan(0).WithMessage("Page number must be greater than 0.");
		RuleFor(_ => _.Size).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}