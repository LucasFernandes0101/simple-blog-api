using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using System.Linq.Expressions;

namespace SimpleBlogApi.Domain.Interfaces.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<Post?> GetByIdWithCommentsAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Post>> GetWithCommentsAsync(int page = 1, int maxResults = 10, Expression<Func<Post, bool>>? criteria = default, CancellationToken cancellationToken = default);
}
