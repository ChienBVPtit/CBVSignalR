using CBVSignalR.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Base.Service
{
    public abstract class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseService(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region CREATE
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region UPDATE
        public virtual async Task<TEntity?> UpdateAsync(TKey id, TEntity entity)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return existing;
        }
        #endregion

        #region DELETE
        public virtual async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region GET BY ID
        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }
        #endregion

        #region GET ALL
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        #endregion

        #region PAGING + FILTER
        public virtual async Task<PagedResult<TEntity>> GetAsync(
            PagingFilterRequest request)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            query = ApplyFilter(query, request);
            query = ApplySearch(query, request.Keyword);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }
        #endregion

        #region HOOK METHODS
        protected virtual IQueryable<TEntity> ApplyFilter(
            IQueryable<TEntity> query,
            PagingFilterRequest request)
            => query;

        protected virtual IQueryable<TEntity> ApplySearch(
            IQueryable<TEntity> query,
            string? keyword)
            => query;
        #endregion
    }
}
