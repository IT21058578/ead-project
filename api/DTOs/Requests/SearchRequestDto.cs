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
            // Create an empty filter expression (allow everything)
            Expression<Func<T, bool>> filterExpression = x => true;

            // Apply filters if available
            if (Filters != null)
            {
                foreach (var (filterKey, filter) in Filters)
                {
                    var property = typeof(T).GetProperty(filterKey) ?? throw new ArgumentException($"Invalid filter key: {filterKey}");
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyAccess = Expression.Property(parameter, property);
                    var constant = Expression.Constant(filter.Value);
                    var comparison = filter.Operator switch
                    {
                        Criteria.Equals => Expression.Equal(propertyAccess, constant),
                        Criteria.NotEquals => Expression.NotEqual(propertyAccess, constant),
                        _ => throw new ArgumentException($"Unsupported filter operator: {filter.Operator}")
                    };

                    // Update the filter expression by combining it with the existing one
                    filterExpression = Expression.Lambda<Func<T, bool>>(
                        Expression.AndAlso(filterExpression.Body, comparison),
                        parameter
                    );
                }
            }

            // Construct sorting expression
            Func<IQueryable<T>, IOrderedQueryable<T>>? sortExpression = null;
            if (!string.IsNullOrEmpty(SortBy) && !string.IsNullOrEmpty(SortDirection))
            {
                var property = typeof(T).GetProperty(SortBy) ?? throw new ArgumentException($"Invalid sort key: {SortBy}");
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);

                var keySelector = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

                // Sorting expression based on SortDirection
                sortExpression = SortDirection == "asc"
                    ? (q => q.OrderBy(keySelector))
                    : (q => q.OrderByDescending(keySelector));
            }

            return new PageRequest<T>
            {
                Page = Page,
                PageSize = PageSize,
                FilterExpression = filterExpression,
                SortExpression = sortExpression
            };
        }

    }

}