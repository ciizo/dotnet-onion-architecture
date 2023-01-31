using Microsoft.EntityFrameworkCore;

namespace Banking.Domain.Entities.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }

        void Commit();

        Task CommitAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();
    }
}