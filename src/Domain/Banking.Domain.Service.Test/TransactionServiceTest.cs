using Banking.Domain.Service.TransactionLogic;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Entities;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using Banking.Infrastructure.Share.Constants;

namespace Banking.Domain.Service.Test
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;

        private void SetUp(Account[] initialEntities)
        {
            var dbContextMock = TestHelper.GetDbContext(initialEntities);
            var uow = new UnitOfWork<BankingContext>(dbContextMock.Object);
            _transactionService = new TransactionService(
                new RepositoryEF<Transaction, BankingContext>(uow),
                new RepositoryEF<Account, BankingContext>(uow),
                uow);
        }

        [Fact]
        public async Task Deposit_Success()
        {
            var random = new Random();
            var amount = 1000;
            var id = Guid.NewGuid();
            var account = new Account() { ID = id, IBAN = random.NextInt64(0, 100).ToString(), CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { account };
            SetUp(initialEntities);

            var result = await _transactionService.Deposit(id, amount);

            Assert.Equal(FeeService.ApplyFee(Enums.TransactionType.Deposit, amount), result.Amount);
            Assert.Equal(Enums.TransactionStatus.Success, result.Status);
            Assert.Equal(Enums.TransactionType.Deposit, result.Type);
            Assert.Equal(id, result.DestinationAccountID);
            Assert.Null(result.SourceAccountID);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Deposit_InvalidInput(decimal amount)
        {
            var random = new Random();
            var id = Guid.NewGuid();
            var account = new Account() { ID = id, IBAN = random.NextInt64(0, 100).ToString(), CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { account };
            SetUp(initialEntities);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _transactionService.Deposit(id, amount));
        }
    }
}