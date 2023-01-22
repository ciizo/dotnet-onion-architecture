using Banking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(e =>
            {
                e.HasMany(e => e.TransactionsIn).WithOne(t => t.DestinationAccount).HasForeignKey(e => e.DestinationAccountID).OnDelete(DeleteBehavior.Restrict);
                e.HasMany(e => e.TransactionsOut).WithOne(t => t.SourceAccount).HasForeignKey(e => e.SourceAccountID).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}