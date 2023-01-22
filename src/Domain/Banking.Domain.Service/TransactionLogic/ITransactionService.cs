using Banking.Domain.Service.Dto;

namespace Banking.Domain.Service.TransactionLogic
{
    public interface ITransactionService
    {
        Task<TransactionDto> Deposit(Guid accountId, decimal amount);

        Task<TransactionDto> Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);
    }
}