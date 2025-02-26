using System.Security.Claims;
using Application.Commands;
using MediatR;

namespace Presentation.Endpoints;

public static class PostEndpoints
{
    public static void AddPostEndpoints(this WebApplication app)
    {
        app.MapPost("/posts", async (CreatePostCommand command, HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                return Results.Unauthorized();
            
            if (command.UserId != guidId)
                return Results.Forbid();

            var result = await mediator.Send(command, cancellationToken);
            return Results.Created($"/posts/{result.Id}", result);
        }).RequireAuthorization();

        app.MapPut("/posts/{id:guid}",
            async (UpdatePostCommand command, HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                    return Results.Unauthorized();
                
                if (command.UserId != guidId)
                    return Results.Forbid();

                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            }).RequireAuthorization();
        
        app.MapDelete("/posts/{id:guid}",
            async (DeletePostCommand command, HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                    return Results.Unauthorized();
                
                if (command.UserId != guidId)
                    return Results.Forbid();

                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            }).RequireAuthorization();
        
        app.MapGet("/posts", async (GetPostsByUserQuery query, HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                return Results.Unauthorized();
            
            if (query.UserId != guidId)
                return Results.Forbid();

            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}