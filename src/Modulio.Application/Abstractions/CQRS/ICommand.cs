using MediatR;

namespace Modulio.Application.Abstractions.CQRS
{
    /// <summary>
    /// Represents a command in the application layer.
    /// Commands are used to perform actions that may cause side effects.
    /// </summary>
    public interface ICommand : IRequest { }

    /// <summary>
    /// Represents a command handler that processes commands and returns a response.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command being handled.</typeparam>
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand { }
}