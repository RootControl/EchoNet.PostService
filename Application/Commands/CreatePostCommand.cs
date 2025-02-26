using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record CreatePostCommand(Guid UserId, string Content) : IRequest<PostDto>;