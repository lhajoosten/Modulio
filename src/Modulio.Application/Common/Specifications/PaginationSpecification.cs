using Ardalis.Specification;
using Modulio.Application.Abstractions.Grid;
using System.Linq.Expressions;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// A specification that adds pagination to an existing specification
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public class PaginationSpecification<T> : Specification<T> where T : class
    {
        /// <summary>
        /// Creates a new pagination specification based on an existing specification and page request
        /// </summary>
        /// <param name="specification">The base specification</param>
        /// <param name="pageRequest">The page request</param>
        public PaginationSpecification(ISpecification<T> specification, PageRequest pageRequest)
            : this(specification, (pageRequest.PageIndex - 1) * pageRequest.PageSize, pageRequest.PageSize)
        {
        }

        /// <summary>
        /// Creates a new pagination specification based on an existing specification with explicit skip and take values
        /// </summary>
        /// <param name="specification">The base specification</param>
        /// <param name="skip">The number of items to skip</param>
        /// <param name="take">The number of items to take</param>
        public PaginationSpecification(ISpecification<T> specification, int skip, int take)
        {
            CopyFromSpecification(specification);
            Query.Skip(skip).Take(take);
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

            // Copy ordering
            foreach (var orderExpression in specification.OrderExpressions)
            {
                switch (orderExpression.OrderType)
                {
                    case OrderTypeEnum.OrderBy:
                    Query.OrderBy(orderExpression.KeySelector);
                    break;
                    case OrderTypeEnum.OrderByDescending:
                    Query.OrderByDescending(orderExpression.KeySelector);
                    break;
                }
            }
        }
    }
}