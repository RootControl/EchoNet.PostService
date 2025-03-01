using Application.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration;

public class PostsServiceIntegrationTests(IntegrationTestFixture fixture) : IClassFixture<IntegrationTestFixture>
{
    private readonly IServiceProvider _serviceProvider = fixture.ServiceProvider;
    
    [Fact]
    public async Task CreateAndGetPost_ShouldWorkEndToEnd()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var userId = Guid.NewGuid();
        var createCommand = new CreatePostCommand(userId, "Hello, World!");

        // Act: Create Post
        var postDto = await mediator.Send(createCommand);

        // Act: Get Posts by User
        var posts = await mediator.Send(new GetPostsByUserQuery(userId));

        // Assert
        postDto.Should().NotBeNull();
        postDto.Content.Should().Be("Hello, World!");
        posts.Should().ContainSingle(p => p.Content == "Hello, World!");
    }
    
    [Fact]
    public async Task UpdatePost_ShouldUpdateContent()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var userId = Guid.NewGuid();
        var createCommand = new CreatePostCommand(userId, "Original Post");
        var postDto = await mediator.Send(createCommand);
        var updateCommand = new UpdatePostCommand(postDto.Id, userId, "Updated Post");

        // Act
        var updatedPost = await mediator.Send(updateCommand);

        // Assert
        updatedPost.Content.Should().Be("Updated Post");
        updatedPost.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeletePost_ShouldRemovePost()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var userId = Guid.NewGuid();
        var createCommand = new CreatePostCommand(userId, "Post to Delete");
        var postDto = await mediator.Send(createCommand);
        var deleteCommand = new DeletePostCommand(postDto.Id, userId);

        // Act
        await mediator.Send(deleteCommand);

        // Assert
        var posts = await mediator.Send(new GetPostsByUserQuery(userId));
        posts.Should().BeEmpty();
    }
}