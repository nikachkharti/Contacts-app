using MediatR;

namespace Contacts.Application.Features.People.Commands
{
    public record DeletePersonCommand(string Id) : IRequest<string>;
}
