using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            public Criteria Operator { get; set; } = Criteria.EQUALS;
            public object Value { get; set; } = null!;

            public enum Criteria
            {
                EQUALS,
                NOT_EQUALS,
                GREATER_THAN,
                LESS_THAN,
                GREATER_THAN_OR_EQUAL,
                LESS_THAN_OR_EQUAL,
                CONTAINS,
            }
        }

        public PageRequest<T> ToPageRequest()
        {
            // Create a new IQueryable based on the generic type T
            var query = Enumerable.Empty<T>().AsQueryable();

            // Apply sorting if SortBy is provided
            if (!string.IsNullOrEmpty(SortBy))
            {
                var property = typeof(T).GetProperty(SortBy) ?? throw new ArgumentException($"Invalid SortBy property: {SortBy}");
                var sortExpression = Expression.Property(null, typeof(T), SortBy);
                var sortLambda = Expression.Lambda<Func<T, object>>(sortExpression, null);
                query = SortDirection == "asc" ? query.OrderBy(sortLambda) : query.OrderByDescending(sortLambda);
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
                        Filter.Criteria.EQUALS => query.Where(x => filter.Value.Equals(property.GetValue(x))),
                        Filter.Criteria.NOT_EQUALS => query.Where(x => !filter.Value.Equals(property.GetValue(x))),
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