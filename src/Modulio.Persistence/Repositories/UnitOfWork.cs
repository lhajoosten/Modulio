using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Modulio.Application.Abstractions.Persistence;
using Modulio.Persistence.Context;

namespace Modulio.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModulioDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed = false;

        public UnitOfWork(ModulioDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                _logger.LogWarning("A transaction is already active. Returning existing transaction.");
                return _currentTransaction;
            }

            _logger.LogDebug("Beginning new transaction");
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException("Transaction is not the current active transaction");

            try
            {
                _logger.LogDebug("Committing transaction");
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogDebug("Transaction committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while committing transaction");
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                _logger.LogWarning("No active transaction to rollback");
                return;
            }

            try
            {
                _logger.LogDebug("Rolling back transaction");
                await _currentTransaction.RollbackAsync(cancellationToken);
                _logger.LogDebug("Transaction rolled back successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rolling back transaction");
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public IDbContextTransaction? GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Saving changes to database");
                var result = await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Successfully saved {Count} changes to database", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to database");
                throw;
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _currentTransaction?.Dispose();
                _context?.Dispose();
                _disposed = true;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                }

                if (_context != null)
                {
                    await _context.DisposeAsync();
                }

                _disposed = true;
            }
        }
    }
}
