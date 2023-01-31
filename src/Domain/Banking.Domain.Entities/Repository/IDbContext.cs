using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Banking.Domain.Entities.Repository
{
    public interface IDbContext :
        IInfrastructure<IServiceProvider>,
        IDbContextDependencies,
        IDbSetCache,
        IDbContextPoolable
    {
        DatabaseFacade Database { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}