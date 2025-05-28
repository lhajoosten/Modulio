using Ardalis.Specification;
using Modulio.Application.Abstractions.Grid;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// Base specification for filtered, paged, and sorted queries
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public abstract class FilteredPagedAndSortedSpecification<T> : Specification<T>
    {
        protected FilteredPagedAndSortedSpecification(PagedAndSortedRequest request, string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                ApplySearchTerm(searchTerm);
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                ApplySorting(request.SortBy, request.SortDirection);
            }

            // Apply paging
            Query.Skip((request.PageIndex - 1) * request.PageSize)
                 .Take(request.PageSize);
        }

        /// <summary>
        /// Applies a search term to the specification. Override this method to implement 
        /// search for your entity type.
        /// </summary>
        protected abstract void ApplySearchTerm(string searchTerm);

        /// <summary>
        /// Applies sorting to the specification. Override this method to implement 
        /// sorting for your entity type.
        /// </summary>
        protected abstract void ApplySorting(string sortBy, string sortDirection);
    }
}
