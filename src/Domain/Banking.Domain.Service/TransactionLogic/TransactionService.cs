using Banking.Domain.Entities;
using Banking.Domain.Entities.Repository;
using Banking.Domain.Entities.UnitOfWork;
using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Share.Constants;
using Microsoft.EntityFrameworkCore;

namespace Banking.Domain.Service.TransactionLogic
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction, IDbContext> _repository;
        private readonly IRepository<Account, IDbContext> _accountRepository;

        private readonly IUnitOfWork<IDbContext> _uow;

        public TransactionService(
            IRepository<Transaction, IDbContext> repository,
            IRepository<Account, IDbContext> accountRepository,
            IUnitOfWork<IDbContext> uow)
        {
            _repository = repository;
            _accountRepository = accountRepository;
            _uow = uow;
        }

        public async Task<TransactionDto> Deposit(Guid accountId, decimal amount)
        {
            if (accountId == default)
            {
                throw new ArgumentException("accountId is required");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than 0");
            }

            var entity = new Transaction()
            {
                Type = Enums.TransactionType.Deposit,
                Status = Enums.TransactionStatus.Success,
                DestinationAccountID = accountId,
                CreatedOn = DateTime.UtcNow,
            };
            entity.Amount = FeeService.ApplyFee(entity.Type, amount);
            //TODO maybe need to create transaction type Fee with central account as DestinationAccount

            var destAccount = await _accountRepository.GetByIdAsync(accountId);
            destAccount.Balance += amount;

            _accountRepository.Update(destAccount);
            _repository.Insert(entity);
            await _uow.CommitAsync();

            return TransactionDto.FromEntity(entity);
        }

        public async Task<TransactionDto> Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (fromAccountId == default || toAccountId == default)
            {
                throw new ArgumentException("accountId is required");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than 0");
            }

            Transaction entity = null;
            await _uow.Context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await _uow.BeginTransactionAsync();

                entity = await ProcessTransfer(fromAccountId, toAccountId, amount);

                await _uow.CommitTransactionAsync();
            });

            return TransactionDto.FromEntity(entity);
        }

        public async Task<Transaction> ProcessTransfer(Guid fromAccountId, Guid toAccountId, decimal srcAmount)
        {
            var entity = new Transaction()
            {
                Type = Enums.TransactionType.Transfer,
                Status = Enums.TransactionStatus.Success,
                DestinationAccountID = toAccountId,
                SourceAccountID = fromAccountId,
                CreatedOn = DateTime.UtcNow,
            };
            entity.Amount = FeeService.ApplyFee(entity.Type, srcAmount);
            //TODO maybe need to create transaction type Fee with central account as DestinationAccount

            var srcAccount = await _accountRepository.GetByIdAsync(entity.SourceAccountID);
            if (srcAccount.Balance < srcAmount)
            {
                throw new NotSupportedException("balance is not enough");
            }
            srcAccount.Balance -= srcAmount;
            var destAccount = await _accountRepository.GetByIdAsync(entity.DestinationAccountID);
            destAccount.Balance += entity.Amount;

            _accountRepository.Update(destAccount);
            _repository.Insert(entity);
            await _uow.CommitAsync();

            return entity;
        }
    }
}