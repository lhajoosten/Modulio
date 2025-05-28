using Ardalis.Specification;
using Modulio.Application.Abstractions.Grid;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// Base specification for paged and sorted queries
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public abstract class PagedAndSortedSpecification<T> : Specification<T>
    {
        protected PagedAndSortedSpecification(PagedAndSortedRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                ApplySorting(request.SortBy, request.SortDirection);
            }

            // Apply paging
            Query.Skip((request.PageIndex - 1) * request.PageSize)
                 .Take(request.PageSize);
        }

        /// <summary>
        /// Applies sorting to the specification. Override this method to implement 
        /// sorting for your entity type.
        /// </summary>
        protected abstract void ApplySorting(string sortBy, string sortDirection);
    }
}
