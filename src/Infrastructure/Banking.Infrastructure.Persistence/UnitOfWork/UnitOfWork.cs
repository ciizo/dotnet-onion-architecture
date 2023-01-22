using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await Context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            await Context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}