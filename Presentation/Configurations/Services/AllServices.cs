using Application.Validators;
using FluentValidation;

namespace Presentation.Configurations.Services;

public static class AllServices
{
    public static void AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MongoDB
        services.AddMongoDb();

        // MediatR
        services.RegisterMediatR();

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(CreatePostCommandValidator).Assembly);

        // JWT Authentication
        services.AddJwtConfiguration(configuration);
        services.AddAuthorization();

        // CORS
        services.ConfigureCors();

        // Health Checks
        services.AddHealthChecks();

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}