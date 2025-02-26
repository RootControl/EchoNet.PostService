using Application.Commands;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class GetPostsByUserQueryHandler(IPostRepository postRepository) : IRequestHandler<GetPostsByUserQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository = postRepository;
    
    public async Task<IEnumerable<PostDto>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        return posts.Select(p => new PostDto(p));
    }
}