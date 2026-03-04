using FluentAssertions;
using NSubstitute;
using Shouldly;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.Handlers.BlogPosts;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Tests.Mocks.Entities;

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

        var postRepository = Substitute.For<IBlogPostRepository>();
        postRepository.GetWithCommentsAsync(command.Page, command.Size, default, cancellationToken)
            .Returns(Task.FromResult(new PagedResult<BlogPost>(
                fakePosts.Count,
                fakePosts
            )));

        var handler = new GetBlogPostHandler(postRepository);

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

        await postRepository.Received(1).GetWithCommentsAsync(command.Page, command.Size, default, cancellationToken);
    }
}