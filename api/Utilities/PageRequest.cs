using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace api.Utilities
{
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