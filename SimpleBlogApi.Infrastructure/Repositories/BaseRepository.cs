using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace SimpleBlogApi.Infrastructure.Repositories;

public abstract class BaseRepository<T>(
    BlogDbContext context)
    : IBaseRepository<T> where T : BaseEntity
{
    public async Task<PagedResult<T>> GetAsync(
        int page = 1,
        int maxResults = 10,
        Expression<Func<T, bool>>? criteria = default,
        CancellationToken cancellationToken = default)
    {
        page = page == 0 ? 1 : page;
        int count = (page - 1) * maxResults;

        IQueryable<T> query = context.Set<T>().AsQueryable();

        if (criteria is not null)
            query = query.Where(criteria);

        var totalRecords = await query.CountAsync();

        var items = await query.Skip(count)
                               .Take(maxResults)
                               .ToListAsync(cancellationToken);

        return new PagedResult<T>(totalRecords, items);
    }

    public async Task<T?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        return await context.Set<T>().FirstOrDefaultAsync(x => 
            x.Id == id, cancellationToken);
    }

    public async Task<T> AddAsync(
        T entity, 
        CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;

        await context.Set<T>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}