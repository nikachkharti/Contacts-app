using Contacts.API.Extensions;
using Contacts.Infrastructure.Middleware;

namespace Contacts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddControllers();
            builder.AddSwagger();
            builder.AddApplication();
            builder.AddInfrastructure();

            var app = builder.Build();

            app.UseMiddleware<DataSeedingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
