using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Tests.Integration;

public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();
    public IServiceProvider ServiceProvider { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "MongoDb:ConnectionString", _mongoDbContainer.GetConnectionString() },
                { "MongoDb:DatabaseName", "TestDb" },
                { "Jwt:Key", "SuperSecretKey" },
                { "Jwt:Issuer", "PostService" },
                { "Jwt:Audience", "PostService" }
            }!)
            .Build();
        
        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostCommandHandler).Assembly));
        
        ServiceProvider = services.BuildServiceProvider();
        
        var context = ServiceProvider.GetRequiredService<MongoDbContext>();
        await context.Posts.DeleteManyAsync(Builders<Post>.Filter.Empty);
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}