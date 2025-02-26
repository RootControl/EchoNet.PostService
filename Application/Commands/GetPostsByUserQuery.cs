using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record GetPostsByUserQuery(Guid UserId) : IRequest<IEnumerable<PostDto>>;