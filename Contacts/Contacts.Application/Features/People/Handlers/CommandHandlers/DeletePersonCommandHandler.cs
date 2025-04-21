using AutoMapper;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.Features.People.Commands;
using Contacts.Application.Validators.Exceptions;
using MediatR;

namespace Contacts.Application.Features.People.Handlers.CommandHandlers
{
    public class DeletePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
        : IRequestHandler<DeletePersonCommand, string>
    {
        public async Task<string> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await personRepository.Get(p => p.Id == request.Id, cancellationToken);

            if (person is null)
            {
                throw new NotFoundException($"Person with id {request.Id} not found");
            }

            await personRepository.Delete(p => p.Id == request.Id, cancellationToken);
            return person.Id;
        }
    }
}
