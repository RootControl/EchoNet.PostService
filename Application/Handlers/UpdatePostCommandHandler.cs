using Application.Commands;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class UpdatePostCommandHandler(IPostRepository postRepository) : IRequestHandler<UpdatePostCommand, PostDto>
{
    private readonly IPostRepository _postRepository = postRepository;
    
    public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (post == null || request.UserId != post.UserId)
            throw new ArgumentException("Post not found or unauthorized");
        
        post.UpdateContent(request.Content);

        await _postRepository.UpdateAsync(post, cancellationToken);

        return new PostDto(post);
    }
    
    private static void ValidateRequest(UpdatePostCommand request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Post Id is required");
        
        if (request.UserId == Guid.Empty)
            throw new ArgumentException("UserId is required");
        
        if (string.IsNullOrEmpty(request.Content))
            throw new ArgumentException("Content is required");
    }
}