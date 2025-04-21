using AutoMapper;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.Features.People.DTOs;
using Contacts.Application.Features.People.Queries;
using Contacts.Application.Validators.Exceptions;
using MediatR;

namespace Contacts.Application.Features.People.Handlers.QueryHandlers
{
    public class GetPersonByIdQueryHandler(IPersonRepository personRepository, IMapper mapper)
        : IRequestHandler<GetPersonByIdQuery, PersonForGettingDto>
    {
        public async Task<PersonForGettingDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await personRepository.Get(p => p.Id == request.PersonId, cancellationToken);

            if (person is null)
            {
                throw new NotFoundException($"Person with id {request.PersonId} not found");
            }

            return mapper.Map<PersonForGettingDto>(person);
        }
    }
}
