using System.Linq.Expressions;

namespace Banking.Domain.Entities.Repository.Base
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool withTracking = true);

        TEntity GetById(object id);

        Task<IEnumerable<TEntity>> GetAsync();

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool withTracking = true);

        Task<TEntity> GetByIdAsync(object id);

        IQueryable<TEntity> GetQueryable();

        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, bool withTracking = true);
    }
}