using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    /// <summary>
    /// The PageMetadata class represents the metadata for a paginated result.
    /// </summary>
    /// 
    /// <remarks>
    /// The PageMetadata class is used to store information about the pagination of a result set.
    /// It contains properties for the current page number, page size, total number of items,
    /// number of items in the current page, total number of pages, and flags indicating if
    /// the current page is the first or last page.
    /// </remarks>
    public class PageMetadata
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; } = 0;
        public int TotalInPage { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public bool IsFirst { get; set; } = false;
        public bool IsLast { get; set; } = false;
    }
}