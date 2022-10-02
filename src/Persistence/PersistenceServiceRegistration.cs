using Application.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence.Configurations;
using Persistence.Repositories.EntityFramework;
using Persistence.Repositories.EntityFramework.Contexts;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoDatabase>(options =>
        {
            var settings = configuration.GetSection("MongoDBSettings").Get<MongoSettings>();
            var client = new MongoClient(settings.ConnectionString);
            return client.GetDatabase(settings.CollectionName);
        });

        services.AddDbContext<BaseDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("templatesql")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}