using MediatR;

namespace Contacts.Application.Features.People.Commands
{
    public record UpdatePersonCommand(string Id, string FullName, DateTime DateOfBirth) : IRequest<string>;
}
