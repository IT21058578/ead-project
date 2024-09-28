using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    public class Page<T>
    {
        public Metadata Meta { get; set; } = new Metadata();
        public IEnumerable<T> Data { get; set; } = [];
        public class Metadata
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
}