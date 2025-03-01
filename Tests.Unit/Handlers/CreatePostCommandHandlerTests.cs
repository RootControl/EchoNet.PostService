using Application.Commands;
using Application.DTOs;
using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Unit.Handlers;

public class CreatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _repoMock;
    private readonly CreatePostCommandHandler _handler;

    public CreatePostCommandHandlerTests()
    {
        _repoMock = new Mock<IPostRepository>();
        _handler = new CreatePostCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsPostAndReturnsDto()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Hello, World!");
        var command = new CreatePostCommand(post.UserId, post.Content);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        _repoMock.Verify(r => r.CreateAsync(It.Is<Post>(p =>
            p.UserId == command.UserId &&
            p.Content == command.Content), cancellationToken), Times.Once);

        result.Should().BeEquivalentTo(new PostDto(post), options => options.Excluding(d => d.Id).Excluding(d => d.CreatedAt).Excluding(d => d.UpdatedAt));
    }
    
    [Fact]
    public async Task Handle_InvalidCommand_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreatePostCommand(Guid.Empty, string.Empty);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
    
    [Fact]
    public async Task Handle_NullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        CreatePostCommand? command = null;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command!, cancellationToken));
    }
}