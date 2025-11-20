using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;

namespace SimpleBlogApi.Infrastructure.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    private readonly BlogDbContext _context;
    public PostRepository(BlogDbContext context) 
        : base(context)
    {
        _context = context;
    }

    public async Task<Post?> GetWithCommentsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
        => await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}