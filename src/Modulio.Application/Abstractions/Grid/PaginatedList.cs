using Microsoft.EntityFrameworkCore;

namespace Modulio.Application.Abstractions.Grid
{
    /// <summary>
    /// Represents a paginated list of items
    /// </summary>
    /// <typeparam name="T">The type of items in the list</typeparam>
    public class PaginatedList<T> : List<T>
    {
        private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// Gets the index of the current page (1-based)
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets the size of a page
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of items
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the total number of pages
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Creates a new paginated list from a queryable source
        /// </summary>
        /// <param name="source">The queryable source</param>
        /// <param name="pageIndex">The index of the page to get (1-based)</param>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="cancellationToken">A token for cancelling the operation</param>
        /// <returns>A new paginated list</returns>
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            // Ensure page index is at least 1
            pageIndex = Math.Max(1, pageIndex);
            // Ensure page size is at least 1
            pageSize = Math.Max(1, pageSize);

            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates a new paginated list from a list of items
        /// </summary>
        /// <param name="source">The list of items</param>
        /// <param name="pageIndex">The index of the page to get (1-based)</param>
        /// <param name="pageSize">The size of a page</param>
        /// <returns>A new paginated list</returns>
        public static PaginatedList<T> Create(
            IList<T> source, int pageIndex, int pageSize)
        {
            // Ensure page index is at least 1
            pageIndex = Math.Max(1, pageIndex);
            // Ensure page size is at least 1
            pageSize = Math.Max(1, pageSize);

            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
