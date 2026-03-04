using FluentAssertions;
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

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdWithCommentsAsync(fakePost.Id, cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var handler = new GetBlogPostDetailHandler(postRepository);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakePost.Id);
        result.Title.Should().Be(fakePost.Title);
        result.Content.Should().Be(fakePost.Content);
        result.Comments.Should().HaveCount(fakePost.Comments.Count);

        await postRepository.Received(1).GetByIdWithCommentsAsync(fakePost.Id, cancellationToken);
    }

    [Fact(DisplayName = "Handle should return null when blog post does not exist")]
    public async Task Handle_ShouldReturnNull_WhenBlogPostDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new GetBlogPostDetailCommand(999);

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetByIdWithCommentsAsync(command.Id, cancellationToken)
            .Returns(Task.FromResult<BlogPost?>(null));

        var handler = new GetBlogPostDetailHandler(postRepository);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().BeNull();

        await postRepository.Received(1).GetByIdWithCommentsAsync(command.Id, cancellationToken);
    }
}
