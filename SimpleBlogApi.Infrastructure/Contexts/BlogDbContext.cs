using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Domain.Base;
using SimpleBlogApi.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleBlogApi.Infrastructure.Contexts;

public class BlogDbContext(
    DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}