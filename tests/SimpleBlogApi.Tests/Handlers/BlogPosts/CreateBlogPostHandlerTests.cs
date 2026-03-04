using FluentAssertions;
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

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.AddAsync(Arg.Any<BlogPost>(), cancellationToken)
            .Returns(Task.FromResult(fakePost));

        var handler = new CreateBlogPostHandler(postRepository);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fakePost.Id);

        await postRepository.Received(1).AddAsync(Arg.Any<BlogPost>(), cancellationToken);
    }
}