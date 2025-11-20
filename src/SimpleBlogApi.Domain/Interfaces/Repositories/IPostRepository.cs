using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Domain.Interfaces.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<Post?> GetWithCommentsByIdAsync(int id, CancellationToken cancellationToken = default);
}
