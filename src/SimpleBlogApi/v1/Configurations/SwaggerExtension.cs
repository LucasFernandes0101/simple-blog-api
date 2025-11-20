using Microsoft.OpenApi;

namespace SimpleBlogApi.v1.Configurations;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Simple Blog API",
                Description = "API for managing posts and comments. Supports creating posts, adding comments, and retrieving posts.",
                Version = "v1"
            });
        });

        return services;
    }
}