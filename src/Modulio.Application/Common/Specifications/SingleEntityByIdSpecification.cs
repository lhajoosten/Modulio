using Ardalis.Specification;
using System.Linq.Expressions;

namespace Modulio.Application.Common.Specifications
{
    /// <summary>
    /// Base specification for retrieving an entity by ID with related entities
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    /// <typeparam name="TId">The type of the entity ID</typeparam>
    public abstract class SingleEntityByIdSpecification<T, TId> : Specification<T>
    {
        protected SingleEntityByIdSpecification(TId id)
        {
            Query.Where(BuildIdPredicate(id));
        }

        /// <summary>
        /// Builds a predicate to find an entity by ID. Override this method to implement
        /// the ID comparison for your entity type.
        /// </summary>
        protected abstract Expression<Func<T, bool>> BuildIdPredicate(TId id);
    }
}
