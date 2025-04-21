using Contacts.Application.Contracts.Repository;
using Contacts.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure.Middleware
{
    public class DataSeedingMiddleware
    {
        private readonly RequestDelegate _next;

        public DataSeedingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();

                var personRepository = scope.ServiceProvider.GetService<IPersonRepository>();

                #region PEOPLE SEED

                var existingPeople = await personRepository.GetAll(pageNumber: 1, pageSize: 1);

                if (!existingPeople.Any())
                {
                    var people = new List<Person>()
                    {
                        new Person()
                        {
                            Id = "68068f8aa851f22ec3099fdd",
                            FullName = "John Lennon",
                            DateOfBirth = DateTime.Parse("1940-10-09")
                        },
                        new Person()
                        {
                            Id = "68068f8aa851f22ec3099fde",
                            FullName = "Paul Maccartney",
                            DateOfBirth = DateTime.Parse("1942-06-18")
                        },
                        new Person()
                        {
                            Id = "68068f8aa851f22ec3099fdf",
                            FullName = "Ringo Starr",
                            DateOfBirth = DateTime.Parse("1940-07-07")
                        },
                        new Person()
                        {
                            Id = "68068f8aa851f22ec3099fe0",
                            FullName = "George Harrison",
                            DateOfBirth = DateTime.Parse("1943-02-25")
                        },
                    };

                    foreach (var person in people)
                    {
                        Console.WriteLine($"Seeding person: {person.FullName}");
                        await personRepository.Insert(person);
                    }

                    Console.WriteLine("People seeding completed");
                }
                else
                {
                    Console.WriteLine("Database already contains people data. Skipping seeding...");
                }

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
