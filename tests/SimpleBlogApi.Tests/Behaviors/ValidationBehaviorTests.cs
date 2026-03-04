using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using SimpleBlogApi.Application.Behaviors;

namespace SimpleBlogApi.Tests.Behaviors;

public class ValidationBehaviorTests
{
    [Fact(DisplayName = "Handle should call next when there are no validators")]
    public async Task Handle_ShouldCallNext_WhenNoValidatorsRegistered()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<TestResponse>>();
        var expectedResponse = new TestResponse();
        next.Invoke(Arg.Any<CancellationToken>()).Returns(expectedResponse);

        // Act
        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        await next.Received(1).Invoke(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle should call next when validation passes")]
    public async Task Handle_ShouldCallNext_WhenValidationPasses()
    {
        // Arrange
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var behavior = new ValidationBehavior<TestRequest, TestResponse>([validator]);

        var next = Substitute.For<RequestHandlerDelegate<TestResponse>>();
        var expectedResponse = new TestResponse();
        next.Invoke(Arg.Any<CancellationToken>()).Returns(expectedResponse);

        // Act
        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        await next.Received(1).Invoke(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle should throw ValidationException when validation fails")]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var failures = new[]
        {
            new ValidationFailure("Property", "Error message")
        };

        var validator = Substitute.For<IValidator<TestRequest>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(failures));

        var behavior = new ValidationBehavior<TestRequest, TestResponse>([validator]);

        var next = Substitute.For<RequestHandlerDelegate<TestResponse>>();

        // Act
        var act = async () => await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Count() == failures.Length);

        await next.DidNotReceive().Invoke(Arg.Any<CancellationToken>());
    }

    public record TestRequest : IRequest<TestResponse>;
    public record TestResponse;
}
