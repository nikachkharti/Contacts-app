using Contacts.Application.Contracts.Repository;
using Contacts.Domain.Entities;
using Contacts.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Contacts.Infrastructure.Repository
{
    public class PersonRepository : MongoRepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(IOptions<MongoDbSettings> options) : base(options, "people")
        {
        }
    }
}
