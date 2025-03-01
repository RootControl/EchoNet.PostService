using Application.Commands;
using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.Unit.Handlers;

public class DeletePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _repoMock;
    private readonly DeletePostCommandHandler _handler;

    public DeletePostCommandHandlerTests()
    {
        _repoMock = new Mock<IPostRepository>();
        _handler = new DeletePostCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_DeletesPost()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Test Post");
        
        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>())).ReturnsAsync(post);
        var command = new DeletePostCommand(post.Id, post.UserId);
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(post.Id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_PostNotFound_ThrowsUnauthorized()
    {
        // Arrange
        var post = new Post(Guid.NewGuid(), "Test Post");
        
        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Post)null!);
        
        var command = new DeletePostCommand(post.Id, post.UserId);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
}