using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace SimpleBlogApi.Infrastructure.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    private readonly BlogDbContext _context;
    public PostRepository(BlogDbContext context) 
        : base(context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdWithCommentsAsync(
        int id,
        CancellationToken cancellationToken = default)
        => await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<PagedResult<Post>> GetWithCommentsAsync(
        int page = 1,
        int maxResults = 10,
        Expression<Func<Post, bool>>? criteria = default,
        CancellationToken cancellationToken = default)
    {
        page = page == 0 ? 1 : page;
        var count = (page - 1) * maxResults;

        var query = _context.Posts.AsQueryable();

        if (criteria is not null)
            query = query.Where(criteria);

        var totalRecords = await query.CountAsync(cancellationToken);

        var items = await query.OrderByDescending(p => p.CreatedAt)
                               .Skip(count)
                               .Take(maxResults)
                               .Include(p => p.Comments)
                               .ToListAsync(cancellationToken);

        return new PagedResult<Post>(totalRecords, items);
    }
}