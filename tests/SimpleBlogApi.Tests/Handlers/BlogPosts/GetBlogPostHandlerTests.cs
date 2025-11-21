using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Shouldly;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Handlers.BlogPosts;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Tests.Mocks.Entities;
using System.Linq.Expressions;

namespace SimpleBlogApi.Tests.Handlers.BlogPosts;

public class GetBlogPostHandlerTests
{
    [Fact(DisplayName = "Handle should return paged blog posts successfully")]
    public async Task Handle_ShouldReturnPagedBlogPosts_WhenCommandIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var fakePosts = new BlogPostFaker(commentsCount: 2).Generate(10);

        var command = new GetBlogPostCommand(1, 10);

        var validator = Substitute.For<IValidator<GetBlogPostCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetWithCommentsAsync(command.Page, command.Size, default, cancellationToken)
            .Returns(Task.FromResult(new PagedResult<BlogPost>(
                fakePosts.Count,
                fakePosts
            )));

        var handler = new GetBlogPostHandler(postRepository, validator);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Total.Should().Be(fakePosts.Count);
        result.Items.Should().HaveCount(fakePosts.Count);

        for (int i = 0; i < fakePosts.Count; i++)
        {
            result.Items[i].Id.Should().Be(fakePosts[i].Id);
            result.Items[i].Title.Should().Be(fakePosts[i].Title);
            result.Items[i].Content.Should().Be(fakePosts[i].Content);
            result.Items[i].CommentCount.ShouldBe(fakePosts[i].Comments.Count);
        }

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).GetWithCommentsAsync(command.Page, command.Size, default, cancellationToken);
    }

    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new GetBlogPostCommand(0, 0);

        var validationFailures = new[]
        {
            new ValidationFailure("Page", "Page must be greater than 0"),
            new ValidationFailure("Size", "Size must be greater than 0")
        };

        var validator = Substitute.For<IValidator<GetBlogPostCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        var postRepository = Substitute.For<IBlogPostRepository>();
        var handler = new GetBlogPostHandler(postRepository, validator);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Count() == validationFailures.Length);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.DidNotReceive().GetWithCommentsAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Expression<Func<BlogPost, bool>>>(), cancellationToken);
    }
}