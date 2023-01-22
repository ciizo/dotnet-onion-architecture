using Banking.Domain.Service.AccountLogic;
using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Entities;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using DeepEqual.Syntax;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;

namespace Banking.Domain.Service.Test
{
    public class AccountServiceTest
    {
        private AccountService _accountService;

        private void SetUp(Account[] initialEntities)
        {
            var _dbContextMock = GetDbContext(initialEntities);
            _accountService = new AccountService(new RepositoryEF<Account>(_dbContextMock.Object), new UnitOfWork<BankingContext>(_dbContextMock.Object));
        }

        private DbContextMock<BankingContext> GetDbContext(Account[] initialEntities)
        {
            var dbContextMock = new DbContextMock<BankingContext>(new DbContextOptionsBuilder<BankingContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.Accounts, initialEntities);
            return dbContextMock;
        }

        [Fact]
        public async Task Get_ReturnAll()
        {
            var random = new Random();
            var id = Guid.NewGuid();
            var account = new Account() { ID = id, IBAN = random.NextInt64(0, 100).ToString(), CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { account };
            SetUp(initialEntities);

            var result = await _accountService.GetAccountById(id);

            AccountDto.FromEntity(account).ShouldDeepEqual(result);
        }
    }
}