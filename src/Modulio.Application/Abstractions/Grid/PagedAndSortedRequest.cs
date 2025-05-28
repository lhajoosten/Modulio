namespace Modulio.Application.Abstractions.Grid
{
    /// <summary>
    /// Represents a request for a page of items with sorting options
    /// </summary>
    public class PagedAndSortedRequest : PageRequest
    {
        /// <summary>
        /// Creates a new paged and sorted request
        /// </summary>
        /// <param name="pageIndex">The index of the page to get (1-based)</param>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="sortBy">The property to sort by</param>
        /// <param name="sortDirection">The sort direction (asc or desc)</param>
        public PagedAndSortedRequest(int pageIndex = 1, int pageSize = 10, string sortBy = null!, string sortDirection = "asc")
            : base(pageIndex, pageSize)
        {
            SortBy = sortBy;
            SortDirection = sortDirection?.ToLower() == "desc" ? "desc" : "asc";
        }

        /// <summary>
        /// Gets the property to sort by
        /// </summary>
        public string SortBy { get; }

        /// <summary>
        /// Gets the sort direction (asc or desc)
        /// </summary>
        public string SortDirection { get; }

        /// <summary>
        /// Gets a value indicating whether the sort direction is descending
        /// </summary>
        public bool IsDescending => SortDirection == "desc";
    }
}
