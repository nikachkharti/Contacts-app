using MediatR;

namespace Contacts.Application.Contracts.CQRS
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<out TResposne> : IRequest<TResposne>
    {
    }
}
