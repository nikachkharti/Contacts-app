using Contacts.Application.Features.People.DTOs;
using MediatR;

namespace Contacts.Application.Features.People.Queries
{
    public record GetAllPeopleQuery
    (
        int? PageNumber,
        int? PageSize,
        string SortingParameter,
        bool Ascending = false,
        CancellationToken CancellationToken = default
    ) : IRequest<IEnumerable<PersonForGettingDto>>;
}
