using Contacts.Domain.Entities;

namespace Contacts.Application.Contracts.Repository
{
    public interface IPersonRepository : IMongoRepositoryBase<Person>
    {
    }
}
