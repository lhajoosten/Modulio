using Ardalis.Specification;
using Modulio.Domain.Base;

namespace Modulio.Application.Abstractions.Persistence
{
    /// <summary>
    /// Read-only repository with AutoMapper projection support
    /// </summary>
    public interface IQueryRepository<T> where T : class, IAggregateRoot
    {
        // Basic queries
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<TDto?> GetByIdAsync<TDto>(object id, CancellationToken cancellationToken = default) where TDto : class;

        // List operations
        Task<List<T>> ListAsync(CancellationToken cancellationToken = default);
        Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<List<TDto>> ListAsync<TDto>(CancellationToken cancellationToken = default) where TDto : class;
        Task<List<TDto>> ListAsync<TDto>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TDto : class;

        // First/Single operations
        Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<TDto?> FirstOrDefaultAsync<TDto>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TDto : class;

        // Counting and existence
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        // Paged results
        Task<(List<T> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<(List<T> Items, int TotalCount)> GetPagedAsync(ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<(List<TDto> Items, int TotalCount)> GetPagedAsync<TDto>(int pageIndex, int pageSize, CancellationToken cancellationToken = default) where TDto : class;
        Task<(List<TDto> Items, int TotalCount)> GetPagedAsync<TDto>(ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken = default) where TDto : class;
    }
}