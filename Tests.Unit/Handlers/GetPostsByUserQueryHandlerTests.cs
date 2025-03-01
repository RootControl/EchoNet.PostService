using FluentAssertions;
using Moq;
using Application.Commands;
using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;

namespace Tests.Unit.Handlers;

public class GetPostsByUserQueryHandlerTests
{
    private readonly Mock<IPostRepository> _repoMock;
    private readonly GetPostsByUserQueryHandler _handler;

    public GetPostsByUserQueryHandlerTests()
    {
        _repoMock = new Mock<IPostRepository>();
        _handler = new GetPostsByUserQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidUserId_ReturnsUserPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var posts = new List<Post>
        {
            new Post(Guid.NewGuid(), "Post 1"),
            new Post(Guid.NewGuid(), "Post 2")
        };
        
        _repoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(posts);
        
        var query = new GetPostsByUserQuery(userId);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Content == "Post 1");
        result.Should().Contain(p => p.Content == "Post 2");
        
        _repoMock.Verify(r => r.GetByUserIdAsync(userId, cancellationToken), Times.Once);
    }
}