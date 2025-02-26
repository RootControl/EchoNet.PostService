using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record UpdatePostCommand(Guid Id, Guid UserId, string Content) : IRequest<PostDto>;