using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.Mappers.Comments;
using SimpleBlogApi.Application.Mappers.Posts;
using SimpleBlogApi.Application.Validators.Posts;
using SimpleBlogApi.Domain.Interfaces.Repositories;
using SimpleBlogApi.Infrastructure.Contexts;
using SimpleBlogApi.Infrastructure.Repositories;
using System.Net.ServerSentEvents;
using System.Reflection;

namespace SimpleBlogApi.Application.Configurations;

public static class DependencyResolver
{
    public static IServiceCollection ResolveDependencies(
        this IServiceCollection services)
    {
        services.ResolveRepositories();
        services.ResolveValidators();
        services.ResolveMediatR();

        return services;
    }

    private static void ResolveRepositories(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");

        ArgumentNullException.ThrowIfNull(
            connectionString, 
            "Environment variable 'SQLSERVER_CONNECTION_STRING' is not set."
        );

        services.AddDbContext<BlogDbContext>(_ =>
            _.UseSqlServer(
                connectionString, 
                _ => _.CommandTimeout(180))
            .EnableSensitiveDataLogging(true));

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
    }

    private static void ResolveValidators(this IServiceCollection services)
    {
        #region Validators Posts
        services.AddScoped<IValidator<CreatePostCommand>, CreatePostCommandValidator>();
        #endregion
    }

    private static void ResolveAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PostProfile));
        services.AddAutoMapper(typeof(CommentProfile));
    }

    private static void ResolveMediatR(this IServiceCollection services)
     => services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}