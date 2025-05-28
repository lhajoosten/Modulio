namespace Modulio.Application.Abstractions.Grid
{
    /// <summary>
    /// Represents a request for a page of items
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// Creates a new page request
        /// </summary>
        /// <param name="pageIndex">The index of the page to get (1-based)</param>
        /// <param name="pageSize">The size of a page</param>
        public PageRequest(int pageIndex = 1, int pageSize = 10)
        {
            PageIndex = Math.Max(1, pageIndex);
            PageSize = Math.Clamp(pageSize, 1, 100); // Limit page size to prevent performance issues
        }

        /// <summary>
        /// Gets the index of the page to get (1-based)
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets the size of a page
        /// </summary>
        public int PageSize { get; }
    }
}
