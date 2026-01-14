using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using cms_webapi.DTOs;

namespace cms_webapi.Helpers
{
    public static class QueryHelper
    {
        /// <summary>
        /// Applies filters to the query based on Filter list
        /// </summary>
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, List<Filter>? filters)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression? predicate = null;

            // Default filter: IsDeleted = false (if IsDeleted property exists)
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null && (isDeletedProperty.PropertyType == typeof(bool) || isDeletedProperty.PropertyType == typeof(bool?)))
            {
                var isDeletedLeft = Expression.Property(param, isDeletedProperty);
                var isDeletedExp = Expression.Equal(isDeletedLeft, Expression.Constant(false));
                predicate = isDeletedExp;
            }

            if (filters == null || filters.Count == 0)
            {
                if (predicate == null) return query;
                var defaultLambda = Expression.Lambda<Func<T, bool>>(predicate, param);
                return query.Where(defaultLambda);
            }

            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.Value)) continue;
                
                var property = typeof(T).GetProperty(filter.Column);
                if (property == null) continue;
                
                var left = Expression.Property(param, property);
                Expression? exp = null;

                if (property.PropertyType == typeof(string))
                {
                    var method = filter.Operator switch
                    {
                        "Contains" => typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        "StartsWith" => typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        "EndsWith" => typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        _ => null
                    };
                    
                    if (method != null)
                    {
                        exp = Expression.Call(left, method, Expression.Constant(filter.Value));
                    }
                    else
                    {
                        exp = Expression.Equal(left, Expression.Constant(filter.Value));
                    }
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    if (int.TryParse(filter.Value, out int val))
                    {
                        exp = filter.Operator switch
                        {
                            ">" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                {
                    if (long.TryParse(filter.Value, out long val))
                    {
                        exp = filter.Operator switch
                        {
                            ">" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    if (decimal.TryParse(filter.Value, out decimal val))
                    {
                        exp = filter.Operator switch
                        {
                            ">" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(filter.Value, out DateTime val))
                    {
                        exp = filter.Operator switch
                        {
                            ">" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    if (bool.TryParse(filter.Value, out bool val))
                    {
                        exp = Expression.Equal(left, Expression.Constant(val));
                    }
                }
                else if (property.PropertyType.IsEnum)
                {
                    if (Enum.TryParse(property.PropertyType, filter.Value, true, out var enumVal))
                    {
                        exp = Expression.Equal(left, Expression.Constant(enumVal));
                    }
                }

                if (exp != null)
                {
                    predicate = predicate == null ? exp : Expression.AndAlso(predicate, exp);
                }
            }

            if (predicate == null) return query;
            
            var lambda = Expression.Lambda<Func<T, bool>>(predicate, param);
            return query.Where(lambda);
        }

        /// <summary>
        /// Applies sorting to the query
        /// </summary>
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, string? sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Id";
            }

            // Check if property exists
            var property = typeof(T).GetProperty(sortBy);
            if (property == null)
            {
                sortBy = "Id"; // Default to Id if property doesn't exist
                property = typeof(T).GetProperty(sortBy);
                if (property == null) return query; // If Id doesn't exist, return as is
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.Property(parameter, property);
            var keySelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(T), member.Type),
                member,
                parameter
            );

            bool isDescending = string.IsNullOrWhiteSpace(sortDirection) 
                ? false 
                : sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase) 
                   || sortDirection.Equals("descending", StringComparison.OrdinalIgnoreCase);

            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var call = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), member.Type },
                query.Expression,
                keySelector
            );
            
            return query.Provider.CreateQuery<T>(call);
        }

        /// <summary>
        /// Applies pagination to the query (1-based page number)
        /// </summary>
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 20;
            
            int skip = (pageNumber - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        /// <summary>
        /// Applies all PagedRequest operations (filters, sorting, pagination) to the query
        /// </summary>
        public static IQueryable<T> ApplyPagedRequest<T>(this IQueryable<T> query, PagedRequest request)
        {
            if (request == null) return query;

            query = query.ApplyFilters(request.Filters);
            query = query.ApplySorting(request.SortBy, request.SortDirection);
            query = query.ApplyPagination(request.PageNumber, request.PageSize);

            return query;
        }
    }
}
