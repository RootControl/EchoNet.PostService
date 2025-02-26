using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Presentation.Configurations.Services;

public static class MongoDbConfiguration
{
    public static void AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IPostRepository, PostRepository>();
    }
}