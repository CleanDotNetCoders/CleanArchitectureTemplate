using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            services.AddSingleton<IMongoDatabase>(options => {
                var settings = configuration.GetSection("MongoDBSettings").Get<MongoSettings>();
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.CollectionName);
            });

            return services;
        }
    }
}
