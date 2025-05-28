using Ardalis.Specification;
using System.Linq.Expressions;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// A specification that adds sorting to an existing specification
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public class SortingSpecification<T> : Specification<T> where T : class
    {
        /// <summary>
        /// Creates a new sorting specification based on an existing specification
        /// </summary>
        /// <param name="specification">The base specification</param>
        /// <param name="sortBy">The property to sort by</param>
        /// <param name="sortDirection">The sort direction (asc or desc)</param>
        public SortingSpecification(ISpecification<T> specification, string sortBy, string sortDirection)
        {
            CopyFromSpecification(specification);
            ApplySorting(sortBy, sortDirection);
        }

        private void CopyFromSpecification(ISpecification<T> specification)
        {
            // Copy where clauses
            foreach (var whereExpression in specification.WhereExpressions)
            {
                Query.Where(whereExpression.Filter);
            }

            // Copy includes
            foreach (var includeExpression in specification.IncludeExpressions)
            {
                Query.Include((Expression<Func<T, object>>)includeExpression.LambdaExpression);
            }

            foreach (var includeString in specification.IncludeStrings)
            {
                Query.Include(includeString);
            }

            // Copy pagination - fix the nullable issue
            if (specification.Skip > 0)
            {
                Query.Skip(specification.Skip);
            }

            if (specification.Take > 0)
            {
                Query.Take(specification.Take);
            }
        }

        private void ApplySorting(string sortBy, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return;

            var isDescending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

            try
            {
                var keySelector = BuildPropertySelector(sortBy);

                if (isDescending)
                    Query.OrderByDescending(keySelector!);
                else
                    Query.OrderBy(keySelector!);
            }
            catch
            {
                // If property doesn't exist, ignore sorting
            }
        }

        private Expression<Func<T, object>> BuildPropertySelector(string propertyPath)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = parameter;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                property = Expression.PropertyOrField(property, propertyName);
            }

            var converted = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, parameter);
        }
    }
}