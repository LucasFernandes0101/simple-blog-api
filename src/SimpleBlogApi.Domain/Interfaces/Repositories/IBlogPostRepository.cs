using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using System.Linq.Expressions;

namespace SimpleBlogApi.Domain.Interfaces.Repositories;

public interface IBlogPostRepository : IBaseRepository<BlogPost>
{
    Task<BlogPost?> GetByIdWithCommentsAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<BlogPost>> GetWithCommentsAsync(int page = 1, int maxResults = 10, Expression<Func<BlogPost, bool>>? criteria = default, CancellationToken cancellationToken = default);
}
