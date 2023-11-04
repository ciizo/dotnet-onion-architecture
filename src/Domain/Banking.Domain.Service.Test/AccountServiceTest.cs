using Banking.Domain.Entities;
using Banking.Domain.Entities.Repository;
using Banking.Domain.Service.AccountLogic;
using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using DeepEqual.Syntax;

namespace Banking.Domain.Service.Test
{
    public class AccountServiceTest
    {
        private AccountService _accountService;

        private void SetUp(Account[] initialEntities)
        {
            var dbContextMock = TestHelper.GetDbContext(initialEntities);
            var uow = new UnitOfWork<IDbContext>(dbContextMock.Object);
            _accountService = new AccountService(
                new RepositoryEF<Account, IDbContext>(dbContextMock.Object), uow);
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

        [Fact]
        public async Task Create_Success()
        {
            var accountDto = new AccountDto() { ID = Guid.NewGuid() };
            SetUp(null);

            var result = await _accountService.CreateAccount(accountDto);

            Assert.NotNull(result);
            Assert.Equal(accountDto.ID, result?.ID);
            Assert.NotNull(result?.IBAN);
            Assert.NotEqual("", result?.IBAN);
        }
    }
}