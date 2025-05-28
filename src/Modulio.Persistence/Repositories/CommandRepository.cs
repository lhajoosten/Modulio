using Modulio.Application.Abstractions.Persistence;
using Modulio.Domain.Base;
using Modulio.Persistence.Context;

namespace Modulio.Persistence.Repositories
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class, IAggregateRoot
    {
        private readonly ModulioDbContext _context;

        public CommandRepository(ModulioDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await SaveEntitiesAsync(cancellationToken);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var entityList = entities.ToList();
            await _context.Set<T>().AddRangeAsync(entityList, cancellationToken);
            await SaveEntitiesAsync(cancellationToken);
            return entityList;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await SaveEntitiesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().UpdateRange(entities);
            await SaveEntitiesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await SaveEntitiesAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().RemoveRange(entities);
            await SaveEntitiesAsync(cancellationToken);
        }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
