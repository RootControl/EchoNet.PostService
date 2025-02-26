using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class PostRepository(MongoDbContext mongoDbContext) : IPostRepository
{
    private readonly MongoDbContext _context = mongoDbContext;
    
    public async Task<Post> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Find(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(Post post, CancellationToken cancellationToken)
    {
        await _context.Posts
            .InsertOneAsync(post, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken)
    {
        await _context.Posts
            .ReplaceOneAsync(p => p.Id == post.Id, post, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _context.Posts
            .DeleteOneAsync(p => p.Id == id, cancellationToken);
    }
}