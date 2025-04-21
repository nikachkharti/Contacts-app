using AutoMapper;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.Features.People.Commands;
using Contacts.Application.Validators.Exceptions;
using Contacts.Domain.Entities;
using MediatR;

namespace Contacts.Application.Features.People.Handlers.CommandHandlers
{
    public class UpdatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
        : IRequestHandler<UpdatePersonCommand, string>
    {
        public async Task<string> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await personRepository.Get(p => p.Id == request.Id, cancellationToken);

            if (existingPerson is null)
            {
                throw new NotFoundException($"Person with id {request.Id} not found");
            }

            var updatedDocumentOfPerson = mapper.Map<Person>(request);

            await personRepository.UpdateSingleDocument(p => p.Id == request.Id, updatedDocumentOfPerson, isUpsert: false, cancellationToken);
            return updatedDocumentOfPerson.Id;
        }
    }
}
