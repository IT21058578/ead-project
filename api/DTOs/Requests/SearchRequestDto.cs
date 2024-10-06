using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using api.Annotations.Validation;
using api.Exceptions;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;

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
            public object Value { get; set; } = "";
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
                    var property = typeof(T).GetProperty(filterKey) ?? throw new BadRequestException($"Invalid filter key: {filterKey}");
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyAccess = Expression.Property(parameter, property);

                    // Cant compare JsonElement so have to convert it
                    // Extract filter value from JsonElement if it's of that type
                    var filterValue = filter.Value;
                    if (filter.Value is JsonElement jsonElement)
                    {
                        filterValue = jsonElement.ValueKind switch
                        {
                            JsonValueKind.String when property.PropertyType.IsEnum => Enum.Parse(property.PropertyType, filter.Value.ToString()),// Handle enum
                            JsonValueKind.String => jsonElement.GetString(), // Handle string
                            JsonValueKind.Number when property.PropertyType == typeof(double) => jsonElement.GetDouble(), // Handle double
                            JsonValueKind.Number => jsonElement.GetInt32(),  // Handle int
                            JsonValueKind.True or JsonValueKind.False => jsonElement.GetBoolean(), // Handle bool
                            _ => throw new BadRequestException($"Unsupported JsonElement type: {jsonElement.ValueKind}")
                        };
                    }
                    var constant = Expression.Constant(filterValue);

                    // Handle conversion if the property is an ObjectId and the filter value is a string
                    Expression propertyAccessExpression;
                    if (property.PropertyType == typeof(ObjectId))
                    {
                        // Convert ObjectId to string for comparison
                        var toStringMethod = typeof(ObjectId).GetMethod(nameof(ObjectId.ToString), Type.EmptyTypes);
                        propertyAccessExpression = Expression.Call(propertyAccess, toStringMethod);
                    }
                    else
                    {
                        // We can just access it normally if no conversion is needed
                        propertyAccessExpression = Expression.Property(parameter, property);
                    }

                    Expression comparison = filter.Operator switch
                    {
                        Criteria.Equals => Expression.Equal(propertyAccessExpression, constant),
                        Criteria.NotEquals => Expression.NotEqual(propertyAccessExpression, constant),
                        Criteria.Contains => Expression.Call(
                            typeof(Enumerable),
                            "Contains",
                            [property.PropertyType.GetGenericArguments()[0]], // Specify the type inside the collection
                            propertyAccessExpression,
                            constant),
                        _ => throw new BadRequestException($"Unsupported filter operator: {filter.Operator}")
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
                var property = typeof(T).GetProperty(SortBy) ?? throw new BadRequestException($"Invalid sort key: {SortBy}");
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