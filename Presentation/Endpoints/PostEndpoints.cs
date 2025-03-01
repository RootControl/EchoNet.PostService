using System.Security.Claims;
using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints;

public static class PostEndpoints
{
    public static void AddPostEndpoints(this WebApplication app)
    {
        app.MapPost("/post", async ([FromBody] CreatePostCommand command, HttpContext context, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                return Results.Unauthorized();
            
            if (command.UserId != guidId)
                return Results.Forbid();

            var result = await mediator.Send(command, cancellationToken);
            return Results.Created($"/posts/{result.Id}", result);
        }).RequireAuthorization();

        app.MapPut("/post",
            async ([FromBody] UpdatePostCommand command, HttpContext context, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                    return Results.Unauthorized();
                
                if (command.UserId != guidId)
                    return Results.Forbid();

                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            }).RequireAuthorization();
        
        app.MapDelete("/post",
            async ([FromBody] DeletePostCommand command, HttpContext context, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                    return Results.Unauthorized();
                
                if (command.UserId != guidId)
                    return Results.Forbid();

                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            }).RequireAuthorization();
        
        app.MapGet("/posts/user/{userId:guid}", async (Guid id, HttpContext context, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guidId))
                return Results.Unauthorized();
            
            if (id != guidId)
                return Results.Forbid();

            var result = await mediator.Send(new GetPostsByUserQuery(id), cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}