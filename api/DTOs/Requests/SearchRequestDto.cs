using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Annotations.Validation;
using api.Models;
using api.Utilities;

namespace api.DTOs.Requests
{
    public class SearchRequestDto<T> where T : BaseModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        [RegularExpression("asc|desc", ErrorMessage = "SortDirection must be 'asc' or 'desc'")]
        public string? SortDirection { get; set; } = "asc"; // Default to ascending order
        public Dictionary<string, Filter>? Filters { get; set; } // For custom filtering

        public class Filter
        {
            [Required]
            [ValidEnumValue(typeof(Criteria))]
            public Criteria Operator { get; set; } = Criteria.Equals;
            public object Value { get; set; } = null!;
        }

        public PageRequest<T> ToPageRequest()
        {
            // Create a new IQueryable based on the generic type T
            var query = Enumerable.Empty<T>().AsQueryable();

            // Apply sorting if SortBy is provided
            if (!string.IsNullOrEmpty(SortBy))
            {
                var property = typeof(T).GetProperty(SortBy) ?? throw new ArgumentException($"Invalid SortBy property: {SortBy}");
                query = SortDirection == "asc" ? query.OrderBy(x => property.GetValue(x)) : query.OrderByDescending(x => property.GetValue(x));
            }

            // Apply filters if available
            if (Filters != null)
            {
                foreach (var (filterKey, filter) in Filters)
                {
                    // Validate filter key against properties of T
                    var property = typeof(T).GetProperty(filterKey) ?? throw new ArgumentException($"Invalid filter key: {filterKey}");

                    // Apply filter based on the operator and property type
                    query = filter.Operator switch
                    {
                        Criteria.Equals => query.Where(x => filter.Value.Equals(property.GetValue(x))),
                        Criteria.NotEquals => query.Where(x => !filter.Value.Equals(property.GetValue(x))),
                        _ => throw new ArgumentException($"Unsupported filter operator: {filter.Operator}"),
                    };
                }
            }

            return new PageRequest<T>
            {
                Page = this.Page,
                PageSize = this.PageSize,
                Query = query
            };
        }

    }

}