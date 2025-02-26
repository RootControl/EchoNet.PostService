using Domain.Entities;

namespace Application.DTOs;

public class PostDto(Post post)
{
    public Guid Id { get; set; } = post.Id;
    public Guid UserId { get; set; } = post.UserId;
    public string Content { get; set; } = post.Content;
    public DateTime CreatedAt { get; set; } = post.CreatedAt;
    public DateTime? UpdatedAt { get; set; } = post.UpdatedAt;
}