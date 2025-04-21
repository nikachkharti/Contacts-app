using AutoMapper;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.Features.People.DTOs;
using Contacts.Application.Features.People.Queries;
using Contacts.Application.Helper;
using Contacts.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Contacts.Application.Features.People.Handlers.QueryHandlers
{
    public class GetAllPeopleQueryHandler(IPersonRepository personRepository, IMapper mapper)
        : IRequestHandler<GetAllPeopleQuery, IEnumerable<PersonForGettingDto>>
    {
        public async Task<IEnumerable<PersonForGettingDto>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            var sortExpression = ConfigureSortingExpression(request);

            var people = await personRepository.GetAll
            (
                request.PageNumber ?? 1,
                request.PageSize ?? 10,
                sortBy: sortExpression,
                ascending: request.Ascending,
                cancellationToken
            );

            if (people.Any())
            {
                return mapper.Map<IEnumerable<PersonForGettingDto>>(people);
            }

            return Enumerable.Empty<PersonForGettingDto>();

        }


        private static Expression<Func<Person, object>> ConfigureSortingExpression(GetAllPeopleQuery request)
        {
            Expression<Func<Person, object>> sortExpression;

            if (!string.IsNullOrWhiteSpace(request.SortingParameter))
            {
                sortExpression = ExpressionBuilder.BuildSortExpression<Person>(request.SortingParameter);
            }
            else
            {
                sortExpression = x => x.Id;
            }

            return sortExpression;
        }

    }
}
