using MediatR;

namespace Modulio.Application.Abstractions.CQRS
{
    /// <summary>
    /// Represents a query in the application layer.
    /// Queries are used to retrieve data without causing side effects.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
    public interface IQuery<TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Represents a query handler that processes queries and returns a response.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the query handler.</typeparam>
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse> { }
}
