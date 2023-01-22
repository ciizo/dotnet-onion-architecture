using Banking.Infrastructure.Persistence.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Banking.Infrastructure.Persistence.Repository.EFCore
{
    public interface IRepositoryEF<TEntity, TContext> : IWriteRepository<TEntity>, IReadRepository<TEntity>, IDisposable
        where TEntity : class
    {
        IEnumerable<TEntity> GetInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null);

        IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null, bool withTracking = true);

        Task<IEnumerable<TEntity>> GetIncludeAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null);

        Task<IEnumerable<TEntity>> GetIncludeAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null, bool withTracking = true);
    }
}