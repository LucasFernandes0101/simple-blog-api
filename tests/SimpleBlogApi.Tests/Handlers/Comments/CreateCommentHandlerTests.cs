using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.Handlers.Comments;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Exceptions;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Tests.Mocks.Entities;

namespace SimpleBlogApi.Tests.Handlers.Comments;

public class CreateCommentHandlerTests
{
    [Fact(DisplayName = "Handle should create comment successfully")]
    public async Task Handle_ShouldReturnCreatedComment_WhenCommandIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var fakePost = new BlogPostFaker().Generate();
        var fakeComment = new CommentFaker().Generate();

        var command = new CreateCommentCommand(
            fakePost.Id, 
            fakeComment.Content,
            fakeComment.Author);

        var validator = Substitute.For<IValidator<CreateCommentCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdAsync(fakePost.Id, cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var commentRepository = Substitute.For<ICommentRepository>();
        commentRepository.AddAsync(Arg.Any<Comment>(), cancellationToken)
            .Returns(Task.FromResult(fakeComment));

        var handler = new CreateCommentHandler(commentRepository, postRepository, validator);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakeComment.Id);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).GetByIdAsync(fakePost.Id, cancellationToken);
        await commentRepository.Received(1).AddAsync(Arg.Any<Comment>(), cancellationToken);
    }

    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateCommentCommand(0, string.Empty, string.Empty);

        var validationFailures = new[]
        {
            new ValidationFailure("BlogPostId", "BlogPostId must be greater than 0"),
            new ValidationFailure("Content", "Content is required")
        };

        var validator = Substitute.For<IValidator<CreateCommentCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        var postRepository = Substitute.For<IBlogPostRepository>();
        var commentRepository = Substitute.For<ICommentRepository>();
        var handler = new CreateCommentHandler(commentRepository, postRepository, validator);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Count() == validationFailures.Length);

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>(), cancellationToken);
        await commentRepository.DidNotReceive().AddAsync(Arg.Any<Comment>(), cancellationToken);
    }

    [Fact(DisplayName = "Handle should throw BlogPostNotFoundException when post does not exist")]
    public async Task Handle_ShouldThrowBlogPostNotFoundException_WhenPostDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateCommentCommand(
            999, 
            "Test comment",
            "Test author");

        var validator = Substitute.For<IValidator<CreateCommentCommand>>();
        validator.ValidateAsync(command, cancellationToken)
            .Returns(Task.FromResult(new ValidationResult()));

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdAsync(command.BlogPostId, cancellationToken)
            .Returns(Task.FromResult<BlogPost?>(null));

        var commentRepository = Substitute.For<ICommentRepository>();
        var handler = new CreateCommentHandler(commentRepository, postRepository, validator);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<BlogPostNotFoundException>()
            .WithMessage($"Post with ID {command.BlogPostId} not found.");

        await validator.Received(1).ValidateAsync(command, cancellationToken);
        await postRepository.Received(1).GetByIdAsync(command.BlogPostId, cancellationToken);
        await commentRepository.DidNotReceive().AddAsync(Arg.Any<Comment>(), cancellationToken);
    }
}