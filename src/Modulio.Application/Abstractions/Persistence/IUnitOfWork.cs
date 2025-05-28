using Microsoft.EntityFrameworkCore.Storage;

namespace Modulio.Application.Abstractions.Persistence
{
    /// <summary>
    /// Unit of Work pattern for managing transactions and coordinating multiple repository operations
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets a value indicating whether there is an active transaction
        /// </summary>
        bool HasActiveTransaction { get; }

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the specified transaction
        /// </summary>
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the current active transaction
        /// </summary>
        IDbContextTransaction? GetCurrentTransaction();

        /// <summary>
        /// Saves all pending changes to the database
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}