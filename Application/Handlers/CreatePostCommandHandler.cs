using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class CreatePostCommandHandler(IPostRepository postRepository) : IRequestHandler<CreatePostCommand, PostDto>
{
    private readonly IPostRepository _postRepository = postRepository;
    
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        
        var post = new Post(request.UserId, request.Content);

        await _postRepository.CreateAsync(post, cancellationToken);

        return new PostDto(post);
    }
    
    private static void ValidateRequest(CreatePostCommand request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        if (request.UserId == Guid.Empty)
            throw new ArgumentException("UserId is required");
        
        if (string.IsNullOrEmpty(request.Content))
            throw new ArgumentException("Content is required");
    }
}