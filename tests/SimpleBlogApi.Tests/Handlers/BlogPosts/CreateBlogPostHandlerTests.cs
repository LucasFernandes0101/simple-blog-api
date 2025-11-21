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

public class CreateBlogPostHandlerTests
{
    [Fact(DisplayName = "Handle should create blog post successfully")]
    public async Task Handle_ShouldReturnCreatedBlogPost_WhenCommandIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var fakePost = new BlogPostFaker(commentsCount: 2).Generate();

        var command = new CreateBlogPostCommand(fakePost.Title, fakePost.Content);

        var validator = Substitute.For<IValidator<CreateBlogPostCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.AddAsync(Arg.Any<BlogPost>(), cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var handler = new CreateBlogPostHandler(postRepository, validator);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakePost.Id);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).AddAsync(Arg.Any<BlogPost>(), cancellationToken);
    }

    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateBlogPostCommand(string.Empty, string.Empty);

        var validationFailures = new[]
        {
            new ValidationFailure("Title", "Title is required"),
            new ValidationFailure("Content", "Content is required")
        };

        var validator = Substitute.For<IValidator<CreateBlogPostCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        var postRepository = Substitute.For<IBlogPostRepository>();
        var handler = new CreateBlogPostHandler(postRepository, validator);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Count() == validationFailures.Length);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.DidNotReceive().AddAsync(Arg.Any<BlogPost>(), cancellationToken);
    }
}