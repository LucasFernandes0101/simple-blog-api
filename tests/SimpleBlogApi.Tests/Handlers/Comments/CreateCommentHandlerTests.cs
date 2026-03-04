using FluentAssertions;
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

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdAsync(fakePost.Id, cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var commentRepository = Substitute.For<ICommentRepository>();
        commentRepository.AddAsync(Arg.Any<Comment>(), cancellationToken)
            .Returns(Task.FromResult(fakeComment));

        var handler = new CreateCommentHandler(commentRepository, postRepository);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakeComment.Id);

        await postRepository.Received(1).GetByIdAsync(fakePost.Id, cancellationToken);
        await commentRepository.Received(1).AddAsync(Arg.Any<Comment>(), cancellationToken);
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

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdAsync(command.BlogPostId, cancellationToken)
            .Returns(Task.FromResult<BlogPost?>(null));

        var commentRepository = Substitute.For<ICommentRepository>();
        var handler = new CreateCommentHandler(commentRepository, postRepository);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<BlogPostNotFoundException>()
            .WithMessage($"Post with ID {command.BlogPostId} not found.");

        await postRepository.Received(1).GetByIdAsync(command.BlogPostId, cancellationToken);
        await commentRepository.DidNotReceive().AddAsync(Arg.Any<Comment>(), cancellationToken);
    }
}