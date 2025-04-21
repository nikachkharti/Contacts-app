using MediatR;

namespace Contacts.Application.Features.People.Commands
{
    public record CreatePersonCommand(string FullName, DateTime DateOfBirth) : IRequest<string>;
}
