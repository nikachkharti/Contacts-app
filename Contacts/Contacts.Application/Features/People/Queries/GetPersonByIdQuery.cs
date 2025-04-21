using Contacts.Application.Features.People.DTOs;
using MediatR;

namespace Contacts.Application.Features.People.Queries
{
    public record GetPersonByIdQuery
    (
        string PersonId,
        CancellationToken CancellationToken = default
    ) : IRequest<PersonForGettingDto>;
}
