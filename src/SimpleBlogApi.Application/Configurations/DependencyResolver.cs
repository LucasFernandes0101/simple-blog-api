using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;
using SimpleBlogApi.Infrastructure.Repositories;
using System.Reflection;

namespace SimpleBlogApi.Application.Configurations;

public static class DependencyResolver
{
    public static IServiceCollection ResolveDependencies(
        this IServiceCollection services)
    {
        services.ResolveRepositories();
        services.ResolveMediatR();

        return services;
    }

    private static void ResolveRepositories(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");

        services.AddDbContext<BlogDbContext>(_ =>
            _.UseSqlServer(
                connectionString, 
                _ => _.CommandTimeout(180))
            .EnableSensitiveDataLogging(true));

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
    }

    private static void ResolveMediatR(this IServiceCollection services)
     => services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}