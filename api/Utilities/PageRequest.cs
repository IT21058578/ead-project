using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace api.Utilities
{
    /// <summary>
    /// The PageRequest class represents a request for paginated data.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of the data to be paginated.</typeparam>
    /// 
    /// <remarks>
    /// The PageRequest class is used to specify the page number, page size, filter expression, and sort expression for retrieving paginated data.
    /// It contains properties for the page number, page size, filter expression, and sort expression.
    /// The filter expression is used to specify the filtering logic for the data.
    /// The sort expression is used to specify the sorting logic for the data.
    /// </remarks>
    public class PageRequest<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        // Expression to store the filtering logic
        public Expression<Func<T, bool>>? FilterExpression { get; set; }

        // Expression to store the sorting logic (key and direction)
        public Func<IQueryable<T>, IOrderedQueryable<T>>? SortExpression { get; set; }
    }
}