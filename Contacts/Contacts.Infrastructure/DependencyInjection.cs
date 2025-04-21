using Contacts.Application.Contracts.Repository;
using Contacts.Infrastructure.Configurations;
using Contacts.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure MongoDB settings
            services.Configure<MongoDbSettings>(configuration.GetSection("Mongo"));

            // Register repositories
            services.AddScoped<IPersonRepository, PersonRepository>();
        }
    }
}
