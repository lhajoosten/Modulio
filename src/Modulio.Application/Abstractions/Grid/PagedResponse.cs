namespace Modulio.Application.Abstractions.Grid
{
    /// <summary>
    /// Represents a response containing a page of items
    /// </summary>
    /// <typeparam name="T">The type of items in the page</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Creates a new paged response
        /// </summary>
        /// <param name="items">The items in the page</param>
        /// <param name="pageIndex">The index of the current page (1-based)</param>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="totalCount">The total number of items</param>
        public PagedResponse(IEnumerable<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        /// <summary>
        /// Gets the items in the page
        /// </summary>
        public IEnumerable<T> Items { get; }

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
    }
}
