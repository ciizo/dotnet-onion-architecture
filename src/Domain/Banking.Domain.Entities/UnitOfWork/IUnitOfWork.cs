using Banking.Domain.Entities.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Banking.Domain.Entities.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : IDbContext
    {
        TContext Context { get; }

        void Commit();

        Task CommitAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();
    }
}