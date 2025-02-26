using Presentation.Endpoints;

namespace Presentation.Configurations.Apps;

public static class AllApps
{
    public static void AddAllApps(this WebApplication app)
    {
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.ConfigureExceptionHandler();
        app.ConfigureSecurityHeaders();
        app.AddPostEndpoints();
        app.MapHealthChecks("/health");
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserService API V1"));
    }
}