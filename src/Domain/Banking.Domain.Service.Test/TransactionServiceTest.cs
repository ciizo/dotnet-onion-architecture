using Banking.Domain.Entities;
using Banking.Domain.Entities.Constants;
using Banking.Domain.Entities.Repository;
using Banking.Domain.Service.TransactionLogic;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;

namespace Banking.Domain.Service.Test
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;

        private void SetUp(Account[] initialEntities)
        {
            var dbContextMock = TestHelper.GetDbContext(initialEntities);
            var uow = new UnitOfWork<IDbContext>(dbContextMock.Object);
            _transactionService = new TransactionService(
                new RepositoryEF<Transaction, IDbContext>(dbContextMock.Object),
                new RepositoryEF<Account, IDbContext>(dbContextMock.Object),
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

            Assert.Equal(FeeServiceHelper.ApplyFee(Enums.TransactionType.Deposit, amount), result.Amount);
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

        [Fact]
        public async Task Transfer_Success()
        {
            var random = new Random();
            var amount = 500;
            var srcAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 1000, CreatedOn = DateTime.UtcNow };
            var destAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 0, CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { srcAccount, destAccount };
            SetUp(initialEntities);

            var result = await _transactionService.ProcessTransfer(srcAccount.ID, destAccount.ID, amount);

            Assert.Equal(FeeServiceHelper.ApplyFee(Enums.TransactionType.Transfer, amount), result.Amount);
            Assert.Equal(Enums.TransactionStatus.Success, result.Status);
            Assert.Equal(Enums.TransactionType.Transfer, result.Type);
            Assert.Equal(destAccount.ID, result.DestinationAccountID);
            Assert.Equal(srcAccount.ID, result.SourceAccountID);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Transfer_InvalidAmount(decimal amount)
        {
            var random = new Random();
            var srcAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 1000, CreatedOn = DateTime.UtcNow };
            var destAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 0, CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { srcAccount, destAccount };
            SetUp(initialEntities);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _transactionService.Transfer(srcAccount.ID, destAccount.ID, amount));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "F5561175-191D-4858-3A71-08DAFC614F12")]
        [InlineData("F5561175-191D-4858-3A71-08DAFC614F12", "00000000-0000-0000-0000-000000000000")]
        public async Task Transfer_InvalidAccountID(Guid srcAccountId, Guid destAccountId)
        {
            var random = new Random();
            var amount = 500;
            var srcAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 1000, CreatedOn = DateTime.UtcNow };
            var destAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 0, CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { srcAccount, destAccount };
            SetUp(initialEntities);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _transactionService.Transfer(srcAccountId, destAccountId, amount));
        }

        [Fact]
        public async Task Transfer_InvalidBalance()
        {
            var random = new Random();
            var amount = 1001;
            var srcAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 1000, CreatedOn = DateTime.UtcNow };
            var destAccount = new Account() { ID = Guid.NewGuid(), IBAN = random.NextInt64(0, 100).ToString(), Balance = 0, CreatedOn = DateTime.UtcNow };
            var initialEntities = new[] { srcAccount, destAccount };
            SetUp(initialEntities);

            var ex = await Assert.ThrowsAsync<NotSupportedException>(async () => await _transactionService.ProcessTransfer(srcAccount.ID, destAccount.ID, amount));
        }
    }
}