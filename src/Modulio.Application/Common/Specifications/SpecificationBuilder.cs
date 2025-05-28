using Ardalis.Specification;
using Modulio.Application.Abstractions.Grid;
using System.Linq.Expressions;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// A fluent API for building specifications
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public class SpecificationBuilder<T> where T : class
    {
        private readonly Specification<T> _specification;

        public SpecificationBuilder()
        {
            _specification = new BuilderSpecification<T>();
        }

        /// <summary>
        /// Applies a filter to the specification
        /// </summary>
        public SpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            _specification.Query.Where(predicate);
            return this;
        }

        /// <summary>
        /// Includes a related entity
        /// </summary>
        public SpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
        {
            _specification.Query.Include(includeExpression);
            return this;
        }

        /// <summary>
        /// Includes a related entity using a string path
        /// </summary>
        public SpecificationBuilder<T> Include(string includePath)
        {
            _specification.Query.Include(includePath);
            return this;
        }

        /// <summary>
        /// Orders results by a property
        /// </summary>
        public SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> keySelector)
        {
            _specification.Query.OrderBy(keySelector);
            return this;
        }

        /// <summary>
        /// Orders results by a property in descending order
        /// </summary>
        public SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            _specification.Query.OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// Applies pagination to the specification
        /// </summary>
        public SpecificationBuilder<T> WithPaging(PageRequest pageRequest)
        {
            _specification.Query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize);
            return this;
        }

        /// <summary>
        /// Applies pagination and sorting to the specification
        /// </summary>
        public SpecificationBuilder<T> WithPagedSorting(PagedAndSortedRequest request,
            Dictionary<string, Expression<Func<T, object>>> sortingMap)
        {
            if (!string.IsNullOrWhiteSpace(request.SortBy) && sortingMap.ContainsKey(request.SortBy))
            {
                var keySelector = sortingMap[request.SortBy];
                if (request.IsDescending)
                    _specification.Query.OrderByDescending(keySelector);
                else
                    _specification.Query.OrderBy(keySelector);
            }

            _specification.Query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize);

            return this;
        }

        /// <summary>
        /// Applies search with a filter expression
        /// </summary>
        public SpecificationBuilder<T> Search(string searchTerm,
            Func<string, Expression<Func<T, bool>>> searchExpressionBuilder)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchExpression = searchExpressionBuilder(searchTerm);
                _specification.Query.Where(searchExpression);
            }

            return this;
        }

        /// <summary>
        /// Builds the specification
        /// </summary>
        public ISpecification<T> Build()
        {
            return _specification;
        }

        /// <summary>
        /// Inner specification implementation for the builder
        /// </summary>
        private class BuilderSpecification<TEntity> : Specification<TEntity> where TEntity : class
        {
        }
    }

    /// <summary>
    /// Extension methods for specification building
    /// </summary>
    public static class SpecificationBuilderExtensions
    {
        /// <summary>
        /// Creates a new specification builder
        /// </summary>
        public static SpecificationBuilder<T> BuildSpecification<T>() where T : class
        {
            return new SpecificationBuilder<T>();
        }

        /// <summary>
        /// Creates a new specification builder with an initial filter
        /// </summary>
        public static SpecificationBuilder<T> BuildSpecification<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return new SpecificationBuilder<T>().Where(predicate);
        }
    }
}
