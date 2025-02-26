using Application.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class DeletePostCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (post == null || request.UserId != post.UserId)
            throw new ArgumentException("Post not found or unauthorized");
        
        await _postRepository.DeleteAsync(request.Id, cancellationToken);
    }
}