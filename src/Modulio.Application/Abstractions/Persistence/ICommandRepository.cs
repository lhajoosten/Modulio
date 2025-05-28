using Modulio.Domain.Base;

namespace Modulio.Application.Abstractions.Persistence
{
    /// <summary>
    /// Write repository for commands
    /// </summary>
    public interface ICommandRepository<T> where T : class, IAggregateRoot
    {
        // Write operations
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // Unit of work
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}