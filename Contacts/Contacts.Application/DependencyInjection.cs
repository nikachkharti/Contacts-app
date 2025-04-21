using Contacts.Application.Behaviors;
using Contacts.Application.Features.People.Handlers.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(GetAllPeopleQueryHandler).Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            services.AddAutoMapper(applicationAssembly);
        }
    }
}
