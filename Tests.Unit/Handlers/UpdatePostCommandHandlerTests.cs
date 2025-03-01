using FluentAssertions;
using Moq;
using Application.Commands;
using Application.DTOs;
using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;

namespace Tests.Unit.Handlers;

public class UpdatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _repoMock;
    private readonly UpdatePostCommandHandler _handler;

    public UpdatePostCommandHandlerTests()
    {
        _repoMock = new Mock<IPostRepository>();
        _handler = new UpdatePostCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesPostAndReturnsDto()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Original Content");
        
        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>())).ReturnsAsync(post);
        
        var command = new UpdatePostCommand(post.Id, post.UserId, post.Content);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Post>(), cancellationToken), Times.Once);

        result.Should().BeEquivalentTo(new PostDto(post));
    }

    [Fact]
    public async Task Handle_PostNotFound_ThrowsUnauthorized()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Original Content");
        
        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Post)null!);
        var command = new UpdatePostCommand(post.Id, post.UserId, post.Content);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
    
    [Fact]
    public async Task Handle_InvalidCommand_ThrowsArgumentException()
    {
        // Arrange
        var command = new UpdatePostCommand(Guid.Empty, Guid.Empty, string.Empty);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
    
    [Fact]
    public async Task Handle_NullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        UpdatePostCommand? command = null;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command!, cancellationToken));
    }
    
    [Fact]
    public async Task Handle_Unauthorized_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Original Content");
        
        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>())).ReturnsAsync(post);
        
        var command = new UpdatePostCommand(post.Id, Guid.NewGuid(), post.Content);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
}