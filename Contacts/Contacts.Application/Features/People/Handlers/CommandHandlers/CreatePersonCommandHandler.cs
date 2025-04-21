using AutoMapper;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.Features.People.Commands;
using Contacts.Domain.Entities;
using MediatR;

namespace Contacts.Application.Features.People.Handlers.CommandHandlers
{
    public class CreatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
        : IRequestHandler<CreatePersonCommand, string>
    {
        public async Task<string> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = mapper.Map<Person>(request);

            await personRepository.Insert(person, cancellationToken);
            return person.Id;
        }
    }
}
