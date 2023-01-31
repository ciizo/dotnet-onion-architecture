using Banking.Domain.Entities.Repository;
using Banking.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Banking.Infrastructure.Persistence.Repository.EFCore
{
    public class RepositoryEF<TEntity, TContext> : IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly IUnitOfWork<TContext> _unitOfWork;

        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryEF(IUnitOfWork<TContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get() => _dbSet.AsEnumerable();

        public virtual TEntity GetById(object id) => _dbSet.Find(id);

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void InsertRange(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entitiy)
        {
            _dbSet.Remove(entitiy);
        }

        public virtual void DeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFunc = null, bool withTracking = true)
        {
            var query = _dbSet.Where(predicate);

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool withTracking = true)
        {
            var query = _dbSet.Where(predicate);

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFunc = null)
        {
            var query = _dbSet.AsQueryable();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return query.AsEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool withTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            query = query.Where(predicate);
            if (withTracking == false)
            {
                query = query.Where(predicate).AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, bool withTracking = true)
        {
            var query = _dbSet.Where(predicate);

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetIncludeAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFunc = null)
        {
            var query = _dbSet.AsQueryable();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetIncludeAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFunc = null, bool withTracking = true)
        {
            var query = _dbSet.Where(predicate);

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}