using SimpleBlogApi.Domain.Entities;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;

namespace SimpleBlogApi.Infrastructure.Repositories;

public class CommentRepository(BlogDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
}