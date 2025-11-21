using SimpleBlogApi.Domain.Base;
using System.Linq.Expressions;

namespace SimpleBlogApi.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<PagedResult<T>> GetAsync(int page = 1, int maxResults = 10, Expression<Func<T, bool>>? criteria = default, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
}