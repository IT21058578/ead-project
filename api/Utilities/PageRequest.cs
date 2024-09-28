using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    public class PageRequest<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IQueryable<T> Query { get; set; } = null!;
    }
}