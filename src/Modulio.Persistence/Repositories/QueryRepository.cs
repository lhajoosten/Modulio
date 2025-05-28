using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Modulio.Application.Abstractions.Persistence;
using Modulio.Domain.Base;
using Modulio.Persistence.Context;

namespace Modulio.Persistence.Repositories
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class, IAggregateRoot
    {
        private readonly ModulioDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public QueryRepository(ModulioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _specificationEvaluator = SpecificationEvaluator.Default;
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync([id], cancellationToken);
        }

        public async Task<TDto?> GetByIdAsync<TDto>(object id, CancellationToken cancellationToken = default) where TDto : class
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }

        public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var queryable = ApplySpecification(specification);
            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<List<TDto>> ListAsync<TDto>(CancellationToken cancellationToken = default) where TDto : class
        {
            return await _context.Set<T>()
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TDto>> ListAsync<TDto>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TDto : class
        {
            var queryable = ApplySpecification(specification);
            return await queryable
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var queryable = ApplySpecification(specification);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TDto?> FirstOrDefaultAsync<TDto>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TDto : class
        {
            var queryable = ApplySpecification(specification);
            return await queryable
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var queryable = ApplySpecification(specification, true);
            return await queryable.CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AnyAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var queryable = ApplySpecification(specification, true);
            return await queryable.AnyAsync(cancellationToken);
        }

        public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<T>();
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var countQuery = ApplySpecification(specification, true);
            var totalCount = await countQuery.CountAsync(cancellationToken);

            var itemsQuery = ApplySpecification(specification)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            var items = await itemsQuery.ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<TDto> Items, int TotalCount)> GetPagedAsync<TDto>(int pageIndex, int pageSize, CancellationToken cancellationToken = default) where TDto : class
        {
            var query = _context.Set<T>();
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<TDto> Items, int TotalCount)> GetPagedAsync<TDto>(ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken = default) where TDto : class
        {
            var countQuery = ApplySpecification(specification, true);
            var totalCount = await countQuery.CountAsync(cancellationToken);

            var itemsQuery = ApplySpecification(specification)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            var items = await itemsQuery
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return _specificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
    }
}
