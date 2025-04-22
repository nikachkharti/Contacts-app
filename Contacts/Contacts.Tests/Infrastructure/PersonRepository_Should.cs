using Contacts.Domain.Entities;
using Contacts.Infrastructure.Configurations;
using Contacts.Infrastructure.Repository;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace Contacts.Tests.Infrastructure
{
    public class PersonRepository_Should : IAsyncLifetime
    {
        private readonly MongoDbRunner _mongoRunner;
        private readonly IMongoDatabase _database;
        private readonly PersonRepository _personRepository;

        public PersonRepository_Should()
        {
            _mongoRunner = MongoDbRunner.Start();
            _database = new MongoClient(_mongoRunner.ConnectionString).GetDatabase("TestDb");
            _personRepository = SetupRepository();
        }


        #region CONFIGURATION
        public Task DisposeAsync()
        {
            _mongoRunner.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await SeedTestData();
        }


        private PersonRepository SetupRepository()
        {
            var settings = Options.Create(new MongoDbSettings
            {
                ConnectionString = _mongoRunner.ConnectionString,
                DatabaseName = "TestDb"
            });

            var repository = new PersonRepository(settings);

            // Set in-memory database and collection using reflection
            typeof(MongoRepositoryBase<Person>)
                .GetProperty("Database", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(repository, _database);

            typeof(MongoRepositoryBase<Person>)
                .GetProperty("Collection", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(repository, _database.GetCollection<Person>("people"));

            return repository;
        }

        private async Task SeedTestData()
        {
            var collection = _database.GetCollection<Person>("people");
            await collection.InsertManyAsync(new List<Person>()
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
                }
            });
        }
        #endregion


        #region GET

        [Fact]
        public async Task Return_All_People()
        {
            var result = await _personRepository.GetAll();
            Assert.NotNull(result);

            var people = Assert.IsAssignableFrom<IEnumerable<Person>>(result);
            Assert.Equal(4, people.Count());
        }

        [Fact]
        public async Task Return_Paged_People()
        {
            var result = await _personRepository.GetAll(pageNumber: 1, pageSize: 2);
            Assert.NotNull(result);

            var people = Assert.IsAssignableFrom<IEnumerable<Person>>(result);
            Assert.Equal(2, people.Count());
        }


        [Fact]
        public async Task Return_Single_Person()
        {
            var result = await _personRepository.Get(x => x.Id == "68068f8aa851f22ec3099fdd");
            Assert.NotNull(result);

            var expectedName = "John Lennon";

            Assert.Equal(expectedName, result.FullName);
        }

        #endregion


        #region INSERT

        [Fact]
        public async Task Insert_New_Person()
        {
            var beforeCount = (await _personRepository.GetAll()).Count();
            var newPerson = new Person()
            {
                FullName = "Mick Jagger",
                DateOfBirth = DateTime.Now,
            };

            await _personRepository.Insert(newPerson);

            var afterCount = (await _personRepository.GetAll()).Count();
            Assert.Equal(beforeCount + 1, afterCount);
        }


        [Fact]
        public async Task Insert_Multiple_Persons()
        {
            var beforeCount = (await _personRepository.GetAll()).Count();
            var mickJagger = new Person()
            {
                FullName = "Mick Jagger",
                DateOfBirth = DateTime.Now,
            };

            var jimmyHendrix = new Person()
            {
                FullName = "Jimmy Hendrix",
                DateOfBirth = DateTime.Now,
            };

            await _personRepository.InsertMultiple(new List<Person>()
            {
                mickJagger,
                jimmyHendrix
            });

            var afterCount = (await _personRepository.GetAll()).Count();
            Assert.Equal(beforeCount + 2, afterCount);
        }

        #endregion


        #region UPDATE

        [Fact]
        public async Task Update_Specific_Field()
        {
            // Arrange
            var originalPerson = (await _personRepository.GetAll(c => c.Id == "68068f8aa851f22ec3099fde")).First();
            var newName = "Kurt Cobain";

            // Act
            await _personRepository.UpdateSingleField(x => x.Id == "68068f8aa851f22ec3099fde", c => c.FullName, newName);
            var updatedPerson = await _personRepository.Get(x => x.Id == "68068f8aa851f22ec3099fde");

            // Assert
            Assert.Equal(newName, updatedPerson.FullName);
            Assert.Equal(originalPerson.Id, updatedPerson.Id); // Other fields remain unchanged
        }

        [Fact]
        public async Task Not_Update_If_Client_Not_Found()
        {
            // Arrange
            var beforeCount = (await _personRepository.GetAll()).Count();

            // Act
            await _personRepository.UpdateSingleField(c => c.Id == "68068f8aa851f22ec3099fde", c => c.DateOfBirth, DateTime.Now);

            // Assert
            var afterCount = (await _personRepository.GetAll()).Count();
            Assert.Equal(beforeCount, afterCount); // Count remains unchanged
        }


        [Fact]
        public async Task Update_Multiple_Fields()
        {
            // Arrange
            var originalPerson = (await _personRepository.GetAll(c => c.Id == "68068f8aa851f22ec3099fde")).First();


            var updates = new Dictionary<Expression<Func<Person, object>>, object>()
            {
                { c => c.FullName, "TEST NAME" },
                { c => c.DateOfBirth, DateTime.Parse("1995-04-06") }
            };

            // Act
            await _personRepository.UpdateMultipleFields(c => c.FullName == "Paul Maccartney", updates);
            var updatedClient = (await _personRepository.Get(c => c.Id == "68068f8aa851f22ec3099fde"));

            // Assert
            Assert.Equal("TEST NAME", updatedClient.FullName); // Name is updated
            Assert.Equal(DateTime.Parse("1995-04-06").ToUniversalTime(), updatedClient.DateOfBirth.ToUniversalTime()); // DOB is updated
        }

        #endregion

    }
}
