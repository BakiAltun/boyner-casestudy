using Boyner.CaseStudy.ApplicationCore.Interfaces.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Boyner.CaseStudy.Infrastructure.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            // Add services to the container.
            services.Configure<DefaultMongoDatabaseSettings>(configuration.GetSection("MongoDatabase"));
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            
            return services;
        }
    }
}
