using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Entities;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using Banking.Infrastructure.Share.Constants;

namespace Banking.Domain.Service.TransactionLogic
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryEF<Transaction, BankingContext> _repository;
        private readonly IRepositoryEF<Account, BankingContext> _accountRepository;

        private readonly IUnitOfWork<BankingContext> _uow;

        public TransactionService(
            IRepositoryEF<Transaction, BankingContext> repository,
            IRepositoryEF<Account, BankingContext> accountRepository,
            IUnitOfWork<BankingContext> uow)
        {
            _repository = repository;
            _accountRepository = accountRepository;
            _uow = uow;
        }

        public async Task<TransactionDto> Deposit(Guid toAccountId, decimal amount)
        {
            var entity = new Transaction()
            {
                Type = Enums.TransactionType.Deposit,
                Status = Enums.TransactionStatus.Success,
                DestinationAccountID = toAccountId,
                CreatedOn = DateTime.UtcNow,
            };
            entity.Amount = FeeService.ApplyFee(entity.Type, amount);
            //TODO maybe need to create transaction type Fee with central account as DestinationAccount

            var destAccount = await _accountRepository.GetByIdAsync(toAccountId);
            destAccount.Balance += amount;

            _accountRepository.Update(destAccount);
            _repository.Insert(entity);
            await _uow.CommitAsync();

            return TransactionDto.FromEntity(entity);
        }

        public async Task<TransactionDto> Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}