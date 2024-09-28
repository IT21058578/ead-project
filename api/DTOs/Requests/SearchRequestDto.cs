using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs.Requests
{
    public class SearchRequestDto<T> where T : BaseModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc"; // Default to ascending order
        public Dictionary<string, Filter>? Filters { get; set; } // For custom filtering

        public class Filter
        {
            public string Operator { get; set; } = null!;
            public object Value { get; set; } = null!;
        }
    }
}