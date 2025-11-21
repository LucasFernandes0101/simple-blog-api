using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Handlers.BlogPosts;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Tests.Mocks.Entities;

namespace SimpleBlogApi.Tests.Handlers.BlogPosts;

public class GetBlogPostDetailHandlerTests
{
    [Fact(DisplayName = "Handle should return blog post detail successfully")]
    public async Task Handle_ShouldReturnBlogPostDetail_WhenCommandIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var fakePost = new BlogPostFaker(commentsCount: 2).Generate();

        var command = new GetBlogPostDetailCommand(fakePost.Id);

        var validator = Substitute.For<IValidator<GetBlogPostDetailCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdWithCommentsAsync(fakePost.Id, cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var handler = new GetBlogPostDetailHandler(postRepository, validator);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakePost.Id);
        result.Title.Should().Be(fakePost.Title);
        result.Content.Should().Be(fakePost.Content);
        result.Comments.Should().HaveCount(fakePost.Comments.Count);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).GetByIdWithCommentsAsync(fakePost.Id, cancellationToken);
    }

    [Fact(DisplayName = "Handle should return null when blog post does not exist")]
    public async Task Handle_ShouldReturnNull_WhenBlogPostDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new GetBlogPostDetailCommand(999);

        var validator = Substitute.For<IValidator<GetBlogPostDetailCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdWithCommentsAsync(command.Id, cancellationToken)
            .Returns(Task.FromResult<BlogPost?>(null));

        var handler = new GetBlogPostDetailHandler(postRepository, validator);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().BeNull();

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).GetByIdWithCommentsAsync(command.Id, cancellationToken);
    }

    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new GetBlogPostDetailCommand(0);

        var validationFailures = new[]
        {
            new ValidationFailure("Id", "Id must be greater than 0")
        };

        var validator = Substitute.For<IValidator<GetBlogPostDetailCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        var postRepository = Substitute.For<IBlogPostRepository>();
        var handler = new GetBlogPostDetailHandler(postRepository, validator);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Count() == validationFailures.Length);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.DidNotReceive().GetByIdWithCommentsAsync(Arg.Any<int>(), cancellationToken);
    }
}
