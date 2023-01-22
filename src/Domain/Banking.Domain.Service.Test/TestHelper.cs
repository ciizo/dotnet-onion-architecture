using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;

namespace Banking.Domain.Service.Test
{
    public class TestHelper
    {
        internal static DbContextMock<BankingContext> GetDbContext(Account[] initialEntities)
        {
            var dbContextMock = new DbContextMock<BankingContext>(new DbContextOptionsBuilder<BankingContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.Accounts, initialEntities);
            dbContextMock.CreateDbSetMock(x => x.Transactions, null);
            return dbContextMock;
        }
    }
}