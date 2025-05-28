using Ardalis.Specification;
using Modulio.Application.Abstractions.Grid;
using Modulio.Application.Common.Specifications;
using System.Linq.Expressions;

namespace Modulio.Application.Extensions
{
    /// <summary>
    /// Extension methods for working with Ardalis.Specification
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Creates a specification that applies pagination
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="spec">The base specification</param>
        /// <param name="pageRequest">The page request</param>
        /// <returns>A new specification with pagination applied</returns>
        public static ISpecification<T> WithPaging<T>(this ISpecification<T> spec, PageRequest pageRequest)
            where T : class
        {
            return new PaginationSpecification<T>(spec, pageRequest);
        }

        /// <summary>
        /// Creates a specification that applies sorting
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="spec">The base specification</param>
        /// <param name="sortBy">The property to sort by</param>
        /// <param name="sortDirection">The sort direction (asc or desc)</param>
        /// <returns>A new specification with sorting applied</returns>
        public static ISpecification<T> WithSorting<T>(this ISpecification<T> spec, string sortBy, string sortDirection)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return spec;

            return new SortingSpecification<T>(spec, sortBy, sortDirection);
        }

        /// <summary>
        /// Creates a specification that applies pagination and sorting
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="spec">The base specification</param>
        /// <param name="request">The paged and sorted request</param>
        /// <returns>A new specification with pagination and sorting applied</returns>
        public static ISpecification<T> WithPagedSorting<T>(this ISpecification<T> spec, PagedAndSortedRequest request)
            where T : class
        {
            var result = spec;

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                result = result.WithSorting(request.SortBy, request.SortDirection);
            }

            result = result.WithPaging(request);

            return result;
        }

        /// <summary>
        /// Creates a specification builder from an existing specification
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="spec">The base specification</param>
        /// <returns>A specification builder that can be further configured</returns>
        public static SpecificationBuilder<T> ToBuilder<T>(this ISpecification<T> spec)
            where T : class
        {
            var builder = new SpecificationBuilder<T>();

            // Apply all conditions from the existing specification
            foreach (var whereExpression in spec.WhereExpressions)
            {
                builder.Where(whereExpression.Filter);
            }

            foreach (var includeExpression in spec.IncludeExpressions)
            {
                builder.Include((Expression<Func<T, object>>)includeExpression.LambdaExpression);
            }

            foreach (var includeString in spec.IncludeStrings)
            {
                builder.Include(includeString);
            }

            return builder;
        }
    }
}